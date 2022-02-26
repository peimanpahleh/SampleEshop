
namespace Baskets.IntegrationTests.Fixtures
{
    public class RedisFixture : IDisposable
    {
        public ServiceProvider ServiceProvider { get; private set; }

        public RedisFixture()
        {
            AddDependency();
        }

        private void AddDependency()
        {

            var services = new ServiceCollection();

            services.AddSingleton(sp =>
            {
                var configuration = ConfigurationOptions.Parse("127.0.0.1", true);

                return ConnectionMultiplexer.Connect(configuration);
            });

            services.AddScoped<IBasketRepository, RedisBasketRepository>();


            services.AddLogging();


            ServiceProvider = services.BuildServiceProvider();
        }

        public void ResetState()
        {

        }

        public void Dispose()
        {
            
        }
    }
}
