namespace Baskets.Api.Models.Response;

public record SuccessIdResult<T>(
    int Code,
    string Msg,
    T Id);
