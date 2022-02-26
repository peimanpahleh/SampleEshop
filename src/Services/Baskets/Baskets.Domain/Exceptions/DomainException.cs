namespace Baskets.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string msg) : base(msg)
    {

    }
}

public class InvalidBasketException : DomainException
{
    public InvalidBasketException(string msg) : base(msg)
    {
    }
}

public class InvalidBasketItemException : DomainException
{
    public InvalidBasketItemException(string msg) : base(msg)
    {
    }
}