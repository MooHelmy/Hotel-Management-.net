public record ServicesResponse(bool Success = false, string Message = null!);
public record ServicesResponse<T>(bool Success, string Message, T? Data = default);
