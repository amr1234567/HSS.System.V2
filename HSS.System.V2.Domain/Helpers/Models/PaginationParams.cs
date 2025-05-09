namespace HSS.System.V2.Domain.Helpers.Models
{
    public record PaginationParams
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }
}