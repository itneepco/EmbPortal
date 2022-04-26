using EmbPortal.Shared.Responses;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.Extensions
{
    public static class ResultExtensions
    {
        public static async Task<IResult<T>> ToResult<T>(this HttpResponseMessage response)
        {
            var responseAsString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorObject = JsonSerializer.Deserialize<ErrorResponse>(responseAsString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return new Result<T>
                {
                    Succeeded = false,
                    Errors = errorObject.Errors ?? new List<string>(),
                    Message = errorObject.Message
                };
            }


            var responseObject = JsonSerializer.Deserialize<T>(responseAsString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return new Result<T>
            {
                Data = responseObject,
                Succeeded = true,
                Message = "Completed the task successfully"
            };
        }

        // In case of return from server with no content
        public static async Task<IResult> ToResult(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync();
                var errorObject = JsonSerializer.Deserialize<ErrorResponse>(responseAsString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return new Result
                {
                    Succeeded = false,
                    Errors = errorObject.Errors ?? new List<string>(),
                    Message = errorObject.Message
                };
            }

            if (!response.IsSuccessStatusCode)
            {
                return new Result
                {
                    Succeeded = false,
                    Message = "An error occured. Please try again later"
                };
            }

            return new Result
            {
                Message = "Completed the task successfully",
                Succeeded = true
            };
        }
    }
}
