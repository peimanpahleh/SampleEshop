using Orders.Domain.Buyers;

namespace Orders.UnitTests.Buyers;

public class BuyerTests
{
    [Test]
    public void Should_create_buyer()
    {
        var userId = Guid.NewGuid().ToString();
        var userName = "user01";

        Buyer buyer = new(userId,userName);

        buyer.Should().NotBeNull();
        buyer.Id.Should().NotBeNull();
        buyer.IdentityGuid.Should().Be(userId);
        buyer.Name.Should().Be(userName);

    }

    // VerifyBuyer
    [Test]
    public void When_buyer_verified_should_raise_an_event()
    {
        var orderId = Guid.NewGuid().ToString();

        var buyer = GetBuyer();
        buyer.VerifyBuyer(buyer.Id,orderId);
        buyer.DomainEvents.Should().HaveCount(1);
    }

    private Buyer GetBuyer()
    {
        var userId = Guid.NewGuid().ToString();
        var userName = "user01";
        Buyer buyer = new(userId, userName);
        return buyer;
    }
}
