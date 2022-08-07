using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public async Task<Document<Machine>> ActivateDevice(string licenseId, string deviceFingerprint, string activationToken)
        {
            var request = new RestRequest("machines", Method.Post);

            request.AddHeader("Authorization", $"Bearer {activationToken}");
            request.AddHeader("Content-Type", "application/vnd.api+json");
            request.AddHeader("Accept", "application/vnd.api+json");
            request.AddJsonBody(new
            {
                data = new
                {
                    type = "machine",
                    attributes = new
                    {
                        fingerprint = deviceFingerprint,

                    },
                    relationships = new
                    {
                        license = new
                        {
                            data = new
                            {
                                type = "license",
                                id = licenseId,
                            }
                        }
                    }
                }
            });

            var response = await client.ExecuteAsync<Document<Machine>>(request);
            if (response.Data.Errors.Count > 0)
            {
                var err = response.Data.Errors[0];

                Console.WriteLine("[ERROR] [ActivateDevice] Status={0} Title={1} Detail={2} Code={3}", response.StatusCode, err.Title, err.Detail, err.Code);

                Environment.Exit(1);
            }

            return response.Data;
        }
        public string GenerateLicenseToken(string license_id)
        {
            var request = new RestRequest(
              $"licenses/{license_id}/tokens",
              Method.Post
            );

            request.AddHeader("Content-Type", "application/vnd.api+json");
            request.AddHeader("Accept", "application/vnd.api+json");
            request.AddHeader("Authorization", "Bearer admin-e259cf4d650f73e54e68384dac0cf804aee9dfde6b650d8eca444818185b1d1bv3");

            request.AddJsonBody(new
            {
                data = new
                {
                    type = "tokens",
                    attributes = new
                    {
                        maxActivations = 1
                    }
                }
            });

            var response = client.Execute(request);
            dynamic json = JsonConvert.DeserializeObject(response.Content);
            string activation_token = json["data"]["attributes"]["token"];
            return activation_token;
        }
        public string CreateLicenseFile(string license_id)
        {
            var request = new RestRequest($"licenses/{license_id}/actions/check-out", Method.Get);
            request.AddHeader("Accept", "application/vnd.api+json");
            request.AddHeader("Authorization", $"Bearer admin-e259cf4d650f73e54e68384dac0cf804aee9dfde6b650d8eca444818185b1d1bv3");

            var response = client.Execute(request);

            return response.Content;
        }
        public string DecryptLicenseFile(string path)
        {
            string licenseFile = System.IO.File.ReadAllText(path);
            var encodedPayload = Regex.Replace(licenseFile, Environment.NewLine, String.Empty);
            encodedPayload = Regex.Replace(encodedPayload, "(^-----BEGIN LICENSE FILE-----|-----END LICENSE FILE-----$)", "");
            var payloadBytes = Convert.FromBase64String(encodedPayload);
            var payload = Encoding.UTF8.GetString(payloadBytes);

            var lic = JsonConvert.DeserializeObject<LicenseFile>(payload);

            if (lic.alg != "base64+ed25519")
                Console.WriteLine("Unsupported Algorithm");

            string data = Encoding.UTF8.GetString(Convert.FromBase64String(lic.enc));
            return data;
        }

    }
}
