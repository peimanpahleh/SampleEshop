using Orders.Domain.Events;

namespace Orders.Domain.Buyers;

public class Buyer : Entity, IAggregateRoot
{

    public string IdentityGuid { get; private set; }

    public string Name { get; private set; }


    public Buyer(string identity, string name)
    {
        IdentityGuid = !string.IsNullOrWhiteSpace(identity) ? identity : throw new ArgumentNullException(nameof(identity));
        Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));

        IncreaseVersion();

    }

    public void VerifyBuyer(string buyerId,string orderId)
    {
        AddDomainEvent(new BuyerVerifiedDomainEvent(buyerId,orderId));

        IncreaseVersion();

    }

}
