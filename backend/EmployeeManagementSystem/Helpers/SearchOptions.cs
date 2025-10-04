namespace EmployeeManagementSystem.Helpers
{
    public class SearchOptions
    {
        public string? Search { get; set; }
        public int? PageIndex { get; set; }
        public int PageSize { get; set; } = 10;
    }
}
