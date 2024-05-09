using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudentManagementWebAPI.Data;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using StudentManagementWebAPI.Services;

// Tạo builder cho ứng dụng web
var builder = WebApplication.CreateBuilder(args);

// Thêm các dịch vụ vào container của ứng dụng
builder.Services.AddControllers();

// Cấu hình DbContext để sử dụng Entity Framework Core và kết nối đến cơ sở dữ liệu SQL Server
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<StudentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cấu hình Versioning
// Thêm Versioning để quản lý phiên bản API
builder.Services.AddApiVersioning(options =>
{
    // Thêm header "api-version" vào request header để xác định phiên bản API
    options.ApiVersionReader = new HeaderApiVersionReader("api-version");

    // Nếu không đặt phiên bản nào mặc định, sẽ sử dụng phiên bản 1.0
    options.AssumeDefaultVersionWhenUnspecified = true;

    // Thiết lập phiên bản mặc định là 1.0
    options.DefaultApiVersion = new ApiVersion(1, 0);

    // Hiển thị phiên bản trong response header
    options.ReportApiVersions = true;
});

//Đăng ký Container DI của ứng dụng
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IGradeService, GradeService>();
builder.Services.AddScoped<IPassportService, PassportService>();

// Thêm Swagger để tạo và quản lý tài liệu API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Student Management API", Version = "v1" });
});

// Build ứng dụng
var app = builder.Build();

// Cấu hình pipeline xử lý yêu cầu HTTP
if (app.Environment.IsDevelopment())
{
    // Sử dụng Swagger UI để hiển thị tài liệu API khi đang trong môi trường phát triển
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Management API v1");
    });
}

// Xử lý các exception và trả về thông báo lỗi dưới dạng JSON
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        var ex = context.Features.Get<IExceptionHandlerFeature>();
        if (ex != null)
        {
            // Log exception vào hệ thống
            // var logger = loggerFactory.CreateLogger("ExceptionHandler");
            // logger.LogError($"Unexpected error: {ex.Error}");

            var errorDetails = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error.",
                ExceptionType = ex.Error.GetType().Name,
                ErrorMessage = ex.Error.Message,
                StackTrace = app.Environment.IsDevelopment() ? ex.Error.StackTrace : null
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorDetails));
        }
    });
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
