using MongoDB.Driver;
using Products.Domain.Images;
using Products.Domain.Products;
using Products.Infrastructure.Settings;

namespace Products.Api;

public static class SeedData
{
    public static void Seed(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var mongoDb = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
            var settings = scope.ServiceProvider.GetRequiredService<IOptions<ServiceSettings>>();
            var mongo = settings.Value.ProductSettings.MongoStoreDatabase;
            var imageCollection = mongoDb.GetCollection<Image>(mongo.ImageCollectionName);
            var productCollection = mongoDb.GetCollection<Product>(mongo.ProductCollectionName);
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            var contentRootPath = env.ContentRootPath;
            var random = new Random();
            for (int i = 1; i <= 6; i++)
            {
                var fileName = $"p{i}.png";
                var name = $"p{i}";


                Log.Logger.Information($"inserting {name} ");

                var filePath = Path.Combine(contentRootPath, "Images", fileName);
                if (!File.Exists(filePath))
                {
                    Log.Logger.Information($"path: {filePath}");
                    Log.Logger.Information($"{fileName} does not exsit");

                    continue;
                }

                var findProdcut = FindProduct(productCollection, name);
                if (findProdcut)
                    continue;

                var fileBytes = File.ReadAllBytes(filePath);
                var image = new Image(fileBytes, "image/png", fileBytes.LongLength);
                var imageId = AddImage(imageCollection, image);
                var price = random.Next(1, 20);
                var quantity = random.Next(5, 15);
                AddProduct(productCollection, imageId, name, price, quantity);

                Log.Logger.Information($"inserted {name} ");
            }
        }
    }

    private static string AddImage(IMongoCollection<Image> imageCollection, Image image)
    {
        imageCollection.InsertOne(image);
        return image.Id;
    }


    private static string AddProduct(IMongoCollection<Product> prodcutCollection, string imageId,
        string name, int price, int quantity)
    {
        var findProduct = prodcutCollection.Find(x => x.Name == name).FirstOrDefault();
        if (findProduct != null)
        {
            Log.Logger.Information($"{name} existed");

            return findProduct.Id;
        }

        var prodcut = new Product(name, quantity, price, imageId);
        prodcutCollection.InsertOne(prodcut);
        return prodcut.Id;
    }

    private static bool FindProduct(IMongoCollection<Product> prodcutCollection, string name)
    {
        var findProduct = prodcutCollection.Find(x => x.Name == name).Any();
        return findProduct;
    }
}
