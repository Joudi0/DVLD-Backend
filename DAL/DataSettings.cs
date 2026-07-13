using Microsoft.Extensions.Configuration;
using System.IO;

namespace DAL
{
    public static class DataSettings
    {
        public static string connectionString;

        static DataSettings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
    }
}