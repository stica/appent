using Microsoft.Extensions.Configuration;

namespace Start.Common.Classes
{
    public class CustomConfigurationProvider
    {
        public CustomConfigurationProvider(IConfiguration configuration)
        {
            ConnectionString = configuration.GetSection("DatabaseSettings").GetSection("ConnectionString").Value;
        }

        public string ConnectionString { get; set; }
    }
}
