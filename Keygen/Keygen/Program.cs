using Keygen;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace keygen
{
    class Program
    {
        static async Task Main(string[] args)
        {
            License? license = null;
            Machine? device = null;
            var license_managment = new LicenseManagement("fb252870-86a1-4007-9bc5-c3e127ab7871");
            string license_id = "2b022c6c-cd94-481b-8a14-253b639f3656";

            //Validate License
            var validation = await license_managment.ValidateLicense(license_id);
            if (validation.Meta.Valid)
            {
                Console.WriteLine("[INFO] [ValidateLicense] Valid={0} ValidationCode={1}", validation.Meta.Detail, validation.Meta.code);
            }
            else
            {
                Console.WriteLine("[INFO] [ValidateLicense] Invalid={0} ValidationCode={1}", validation.Meta.Detail, validation.Meta.code);
            }

            //Create License File and save it
            string test = license_managment.CreateLicenseFile(license_id);
            using (FileStream fs = File.Create(@"D:\Learning\KeyGen\Keygen\Keygen\license.txt"))
            {
                char[] value = test.ToCharArray();
                fs.Write(Encoding.UTF8.GetBytes(value), 0, value.Length);
            }

            license = validation.Data;
            //Check if machine is activated
            switch (validation.Meta.code)
            {
                case "FINGERPRINT_SCOPE_MISMATCH":
                case "NO_MACHINES":
                case "NO_MACHINE":
                    var activation_token = license_managment.GenerateLicenseToken(license_id);
                    var activation = await license_managment.ActivateDevice(license_id, "", activation_token);

                    device = activation.Data;

                    Console.WriteLine("[INFO] [ActivateDevice] DeviceId={0} LicenseId={1}", device.ID, license.ID);

                    validation = await license_managment.ValidateLicense(license_id);
                    if (validation.Meta.Valid)
                    {
                        Console.WriteLine("[INFO] [ValidateLicense] Valid={0} ValidationCode={1}", validation.Meta.Detail, validation.Meta.code);
                    }
                    else
                    {
                        Console.WriteLine("[INFO] [ValidateLicense] Invalid={0} ValidationCode={1}", validation.Meta.Detail, validation.Meta.code);
                    }

                    break;
                    Console.WriteLine("[INFO] [Main] Valid={0} RecentlyActivated={1}", validation.Meta.Valid);
            }
        }
    }
}