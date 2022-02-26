
namespace Products.Api.Models.Response;

public record SuccessedResult(
    int Code,
    string Msg);

public record FailedResult(
    int Code,
    string Msg);

public record SuccessIdResult<T>(
    int Code,
    string Msg,
    T Id);
