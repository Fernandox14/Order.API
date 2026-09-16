namespace Order.Aplicacao.Dto;

public class BaseResponseDto<T> where T : class
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new();
}