namespace Baskets.Application.Configuration.Commands;


public interface ICommand<out TResult> : IRequest<TResult>
{
}
