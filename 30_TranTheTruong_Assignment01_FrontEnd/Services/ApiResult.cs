namespace _30_TranTheTruong_Assignment01_FrontEnd.Services
{
    public class ApiResult
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public int Status { get; set; }
    }

    public class ApiResult<T> : ApiResult
    {
        public T? Data { get; set; }

        public static ApiResult<T> Ok(T? data, int status = 200) => new() { Success = true, Data = data, Status = status };
        public static ApiResult<T> Fail(string error, int status) => new() { Success = false, Error = error, Status = status };
    }

    /// <summary>Thrown when the API answers 401 for an authenticated call (token missing / expired).</summary>
    public class ApiUnauthorizedException : Exception
    {
    }

}
