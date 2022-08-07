using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keygen
{
    public class LicenseManagement
    {
        RestClient? client = null;
        public LicenseManagement(string accountId)
        {
            client = new RestClient($"https://api.keygen.sh/v1/accounts/{accountId}");
        }

    }
}
