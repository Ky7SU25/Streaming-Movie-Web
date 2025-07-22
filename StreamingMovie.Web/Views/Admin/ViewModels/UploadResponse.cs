namespace StreamingMovie.Web.Views.Admin.ViewModels;

public class UploadResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public object? Data { get; set; }
    public List<string>? Errors { get; set; }

    public static UploadResponse SuccessResult(string message = "Upload successful", object? data = null)
    {
        return new UploadResponse
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static UploadResponse ErrorResult(string message, List<string>? errors = null)
    {
        return new UploadResponse
        {
            Success = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }

    public static UploadResponse ValidationErrorResult(List<string> validationErrors)
    {
        return new UploadResponse
        {
            Success = false,
            Message = "Validation failed",
            Errors = validationErrors
        };
    }
}