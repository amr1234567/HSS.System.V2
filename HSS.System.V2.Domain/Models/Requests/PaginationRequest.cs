using System.ComponentModel.DataAnnotations;

namespace HSS.System.V2.Domain.Models.Requests
{
    public class PaginationRequest
    {
        private const int DefaultPageSize = 10;
        private const int MaxPageSize = 100;
        private const int MinPageSize = 1;
        private const int MinPage = 1;

        private int _pageSize = DefaultPageSize;
        private int _page = MinPage;

        [Range(MinPage, int.MaxValue, ErrorMessage = "رقم الصفحة يجب أن يكون أكبر من أو يساوي 1")]
        public int Page
        {
            get => _page;
            set => _page = value < MinPage ? MinPage : value;
        }

        [Range(MinPageSize, MaxPageSize, ErrorMessage = "حجم الصفحة يجب أن يكون بين 1 و 100")]
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < MinPageSize ? MinPageSize : value > MaxPageSize ? MaxPageSize : value;
        }
    }
} 