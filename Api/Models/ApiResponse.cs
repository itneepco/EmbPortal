namespace Api.Models
{
   public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; }
        public string Message { get; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request, you have made",
                401 => "You are not authorized",
                403 => "You are forbidden to access this resource",
                404 => "Resource not found",
                500 => "An internal server error occured",
                _ => null
            };
        }
    }
}