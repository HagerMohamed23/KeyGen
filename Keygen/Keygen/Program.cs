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
            License license = null;
            Machine device = null;
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
        }
    }
}