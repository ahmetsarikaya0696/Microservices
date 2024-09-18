using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FreeCourse.Shared.DTOs
{
    public class ResponseDTO<T>
    {
        public T Data { get; set; }

        [JsonIgnore]
        public int StatusCode { get; set; }

        [JsonIgnore]
        public bool IsSuccessfull { get; set; }

        public List<string> Errors { get; set; }

        public static ResponseDTO<T> Success(T data, int statusCode) => new ResponseDTO<T> { Data = data, StatusCode = statusCode, IsSuccessfull = true };

        public static ResponseDTO<T> Success(int statusCode) => new ResponseDTO<T> { Data = default, StatusCode = statusCode, IsSuccessfull = true };

        public static ResponseDTO<T> Fail(List<string> errors, int statusCode) => new ResponseDTO<T> { Errors = errors, StatusCode = statusCode, IsSuccessfull = false };

        public static ResponseDTO<T> Fail(string error, int statusCode) => new ResponseDTO<T> { Errors = new List<string>() { error }, StatusCode = statusCode, IsSuccessfull = false };
    }
}
