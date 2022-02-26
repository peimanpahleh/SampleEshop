namespace Orders.Application.Configuration.Commands;


public interface ICommand<out TResult> : IRequest<TResult>
{
}
