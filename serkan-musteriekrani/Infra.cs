using Microsoft.EntityFrameworkCore;
using serkan_test1.Data;

namespace serkan_test1
{
    public static class Infra
    {
        public static void AddInfra(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = "TestDb";

            services.AddDbContext<UygulamaDbContext>(options =>
                options.UseInMemoryDatabase(connectionString));
        }
    }
}
