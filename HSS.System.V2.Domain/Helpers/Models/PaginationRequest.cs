namespace HSS.System.V2.Domain.Helpers.Models;

public class PaginationRequest
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public PaginationRequest(int page, int size)
    {
        Page = page;
        Size = size;
    }
    public PaginationRequest()
    {
    }
}
