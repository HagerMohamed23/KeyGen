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
        public async Task<Document<License, Validation>> ValidateLicense(string license_id)
        {
            var request = new RestRequest($"licenses/{license_id}/actions/validate", Method.Post);
            request.AddHeader("Content-Type", "application/vnd.api+json");
            request.AddHeader("Accept", "application/vnd.api+json");
            request.AddHeader("Authorization", $"Bearer admin-e259cf4d650f73e54e68384dac0cf804aee9dfde6b650d8eca444818185b1d1bv3");


            var response = await client.ExecuteAsync<Document<License, Validation>>(request);
            if (response.Data.Errors.Count > 0)
            {
                var err = response.Data.Errors[0];

                Console.WriteLine("[ERROR] [ValidateLicense] Status={0} Title={1} Detail={2} Code={3}", response.StatusCode, err.Title, err.Detail, err.Code);
            }

            return response.Data;
        }

    }
}
