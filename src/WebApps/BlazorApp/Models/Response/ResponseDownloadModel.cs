namespace BlazorApp.Models.Response;

public class ResponseDownloadModel
{
    public int Code { get; set; }

    public string Msg { get; set; }

    public byte[] FileBytes { get; set; }

    public string FileName { get; set; }

}


public record ResponseResult<T>(
    int Code,
    string Msg,
    T Id);