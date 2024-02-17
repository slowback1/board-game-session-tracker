namespace Database.Common.DTOs;

public class ApiResponse<T>
{
    public List<string>? Errors { get; set; }
    public T? Response { get; set; }
}