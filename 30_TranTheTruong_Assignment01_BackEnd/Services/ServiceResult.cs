namespace _30_TranTheTruong_Assignment01_BackEnd.Services
{
    public class ServiceResult
    {
        public bool Success { get; init; }
        public bool NotFound { get; init; }
        public string? Error { get; init; }

        public static ServiceResult Ok() => new() { Success = true };
        public static ServiceResult Fail(string error) => new() { Error = error };
        public static ServiceResult Missing(string error = "The requested item was not found.") => new() { NotFound = true, Error = error };
    }

    public record AuthenticatedUser(short Id, string Name, string Email, string Role);

}
