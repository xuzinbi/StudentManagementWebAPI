# STAGE 1: Build the application

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["StudentManagementWebAPI.csproj", "StudentManagementWebAPI/"]
RUN dotnet restore "./StudentManagementWebAPI/StudentManagementWebAPI.csproj"

# Copy everything else and build
WORKDIR "/src/StudentManagementWebAPI"
COPY . .

RUN dotnet build "./StudentManagementWebAPI.csproj" -c Release -o /app/build

# STAGE 2: Publish the application
FROM build AS publish
RUN dotnet publish "./StudentManagementWebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# STAGE 3: Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudentManagementWebAPI.dll"]

