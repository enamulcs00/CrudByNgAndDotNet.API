using CrudByNgAndDotNet.API.Models;

namespace CrudByNgAndDotNet.API.Helper
{
    public static class ApiResponseHelper
    {
        public static ApiResponse<T> SuccessResult<T>(T data, string message = "Request successful")
        {
            return new ApiResponse<T>
            {
                Status = true,
                StatusCode = StatusCodes.Status200OK,
                Message = message,
                Data = data,
                ErrorDetails = null
            };
        }

        public static ApiResponse<string> FailureResult(string message, int statusCode = StatusCodes.Status400BadRequest, object errorDetails = null)
        {
            return new ApiResponse<string>
            {
                Status = false,
                StatusCode = statusCode,
                Message = message,
                Data = null,
                ErrorDetails = errorDetails
            };
        }
    }

}

