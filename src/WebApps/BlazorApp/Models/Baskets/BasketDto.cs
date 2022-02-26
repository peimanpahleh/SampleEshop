namespace BlazorApp.Models.Baskets;

public record BasketDto
{
    public string BuyerId { get; set; }
    public string BuyerName { get; set; }
    public List<BasketItemDto> BasketItems { get; set; } = new();

    public BasketDto()
    {
    }

    public BasketDto(string buyerId, string buyerName, List<BasketItemDto> basketItems)
    {
        BuyerId = buyerId;
        BuyerName = buyerName;
        BasketItems = basketItems;
    }
}



