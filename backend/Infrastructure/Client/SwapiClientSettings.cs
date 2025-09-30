using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Client
{
    public class SwapiClientSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public int TimeoutSeconds { get; set; } = 30;
        public bool UseFakeData { get; set; } = false;
    }
}
