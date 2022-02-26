namespace BlazorApp.Models.Baskets;

public record UpdateBasketModel(List<BasketItemModel> BasketItems);


public record BasketItemModel(
        string ProductId,
        int Quantity);
