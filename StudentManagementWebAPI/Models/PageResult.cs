namespace StudentManagementWebAPI.Models
{
    public class PagedResult<T>
    {
        //Tạo một lớp hoặc một cấu trúc để đóng gói dữ liệu và thông tin phân trang Paging
        //Để Items không bao giờ là null
        public PagedResult()
        {
            Items = new List<T>();
        }

        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)System.Math.Ceiling((double)TotalCount / PageSize);
    }
}
