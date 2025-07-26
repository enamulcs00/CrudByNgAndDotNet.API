namespace CrudByNgAndDotNet.API.Models
{
    public class ApiResponse<T>
    {
        public bool Status { get; set; } // e.g., "True", "False"
        public int StatusCode { get; set; } // HTTP status code
        public string Message { get; set; } // Description of the response
        public T Data { get; set; } // Response data (generic type)
        public object ErrorDetails { get; set; } // Additional error information
    }

}

