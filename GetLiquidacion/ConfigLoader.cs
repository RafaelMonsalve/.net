using System;
using System.Configuration;

namespace GetLiquidacion
{
    public static class ConfigLoader
    {
        public static ConfigSettings Load()
        {
            return new ConfigSettings
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString,
                Liquidacion = ConfigurationManager.AppSettings["liquidacion"],
                TokenTable = ConfigurationManager.AppSettings["token"],
                Interval = int.Parse(ConfigurationManager.AppSettings["interval"]),
                log = ConfigurationManager.AppSettings["log"],
                GetAnticipoTable = ConfigurationManager.AppSettings["getanticipo"],
                ApiUrl = ConfigurationManager.AppSettings["ApiUrl"]
            };
        }
    }

    public class ConfigSettings
    {
        public string ConnectionString { get; set; }
        public string Liquidacion { get; set; }
        public string TokenTable { get; set; }
        public int Interval { get; set; }
        public string log { get; set; }
        public string GetAnticipoTable { get; set; }
        public string ApiUrl {get; set;}
    }
}
