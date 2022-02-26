namespace Orders.IntegrationTests.Repositories;

public class MongoBuyerRepositoryTests : IClassFixture<MongoFixture>
{
    private readonly IBuyerRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public MongoBuyerRepositoryTests(MongoFixture fixture)
    {
        _repo = fixture.ServiceProvider.GetRequiredService<IBuyerRepository>();
        _unitOfWork = fixture.ServiceProvider.GetRequiredService<IUnitOfWork>();
    }

    [Fact]
    public async Task Should_add_buyer()
    {
        var buyer = GetBuyer();

        _repo.Add(buyer);
        await _unitOfWork.SaveChangesAsync();

        var res = await _repo.GetByIdAsync(buyer.Id);
        res.Should().NotBeNull();
        res.Name.Should().Be(buyer.Name);
        res.IdentityGuid.Should().Be(buyer.IdentityGuid);
    }

    private Buyer GetBuyer()
    {
        var userId = Guid.NewGuid().ToString();
        var userName = "user02";

        Buyer buyer = new(userId,userName);
        return buyer;
    }


}
