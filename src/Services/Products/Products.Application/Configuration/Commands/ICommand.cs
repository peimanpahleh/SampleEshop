
namespace Products.Application.Configuration.Commands;


public interface ICommand<out TResult> : IRequest<TResult>
{
}
