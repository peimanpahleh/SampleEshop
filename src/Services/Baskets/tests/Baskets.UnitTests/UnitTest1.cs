using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Baskets.UnitTests;

public class UnitTest1
{


    [Fact]
    public void Validate_group_product()
    {
        var basket = GetStaticBasket();

        var itemId = basket.BasketItems.First().Id;

        var updateBasket = GetStaticBasket();

        updateBasket.ChangeQuantity(itemId, 2);

        basket.BasketItems.First().Quantity.Should().Be(1);
        updateBasket.BasketItems.First().Quantity.Should().Be(2);

    }

    public Basket UpdateBasket(Basket firstBasket, BasketUpdateDto updatedBasket)
    {
        List<BasketItem> itemsShouldBeAdd = new();
        // check exist items
        foreach (var item in updatedBasket.BasketItems)
        {
            var existItem = firstBasket.BasketItems.FirstOrDefault(b => b.ProductId == item.ProductId);
            if (existItem != null)
            {
                if (existItem.Quantity != item.Quantity)
                {
                    // add items
                    firstBasket.ChangeQuantity(existItem.Id, item.Quantity);
                }
            }
            else
            {
                // add new
                /*var newItem = new BasketItem(Guid.NewGuid(),item.ProductId,)
                itemsShouldBeAdd.Add(item);*/
            }
        }

        if (itemsShouldBeAdd.Count > 0)
        {
            firstBasket.AddItems(itemsShouldBeAdd);
        }

        return firstBasket;
    }

    [Fact]
    public void Test1()
    {

    }

    private Basket GetBasket()
    {
        // basket
        var buyerId = Guid.NewGuid();
        var buyerName = "test buyer";

        // basketItem
        var ItemId = Guid.NewGuid();
        var productId = Guid.NewGuid().ToString();
        var productName = "p01";
        var unitPrice = 5;
        var unitPriceId = Guid.NewGuid().ToString();
        var quantity = 1;
        var imageId = Guid.NewGuid().ToString();

        var basket = new Basket(buyerId, buyerName);

        List<BasketItem> basketItems = new();
        var basketItem = new BasketItem(ItemId, productId, productName, unitPrice, unitPriceId, quantity, imageId);
        basketItems.Add(basketItem);
        basket.AddItems(basketItems);

        return basket;
    }

    private Basket GetStaticBasket()
    {
        // basket
        var buyerId = Guid.Parse("8472067C-B7E2-4CB1-91E7-E48BD56A4CDE");
        var buyerName = "test buyer";

        // basketItem
        var ItemId = Guid.Parse("8572067C-B7E2-4CB1-91E7-E48BD56A4CDE");
        var productId = Guid.NewGuid().ToString();
        var productName = "p01";
        var unitPrice = 5;
        var unitPriceId = Guid.NewGuid().ToString();
        var quantity = 1;
        var imageId = Guid.NewGuid().ToString();

        var basket = new Basket(buyerId, buyerName);

        List<BasketItem> basketItems = new();
        var basketItem = new BasketItem(ItemId, productId, productName, unitPrice, unitPriceId, quantity, imageId);
        basketItems.Add(basketItem);
        basket.AddItems(basketItems);

        return basket;
    }



}

public record BasketUpdateDto(
        string BuyerId,
        string BuyerName,
        List<BasketUpdateItemDto> BasketItems);

public record BasketUpdateItemDto(
        string ProductId,
        int Quantity);