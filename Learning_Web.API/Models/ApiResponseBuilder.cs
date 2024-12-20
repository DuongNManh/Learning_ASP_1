using Learning_Web.API.Models.Response;

namespace Learning_Web.API.Models
{
    public static class ApiResponseBuilder
    {
        public static ApiResponse<T> BuildResponse<T>(T data, string message, int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                Data = data,
                Message = message,
                StatusCode = statusCode,
                IsSuccess = statusCode >= 200 && statusCode < 300,
                Reason = null
            };
        }

        public static ApiResponse<T> BuildErrorResponse<T>(string message, string reason, int statusCode)
        {
            return new ApiResponse<T>
            {
                Data = default,
                Message = message,
                StatusCode = statusCode,
                IsSuccess = false,
                Reason = reason
            };
        }

        public static ApiResponse<PageResponse<T>> BuildPageResponse<T>(
            IEnumerable<T> items,
            int totalPages,
            int currentPage,
            int pageSize,
            long totalItems,
            string message)
        {
            var pagedResponse = new PageResponse<T>
            {
                Items = items,
                Meta = new PaginationMeta
                {
                    TotalPages = totalPages,
                    CurrentPage = currentPage,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return new ApiResponse<PageResponse<T>>
            {
                Data = pagedResponse,
                Message = message,
                StatusCode = 200,
                IsSuccess = true,
                Reason = null
            };
        }
    }
}
