using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keygen
{
    public class Document<T>
    {
        public T Data { get; set; }
        public List<Error> Errors { get; set; } = new();
    }

    public class Document<T, U> : Document<T>
    {
        public U Meta { get; set; }
    }

    public class Error
    {
        public string Title { get; set; }
        public string Detail { get; set; }
        public string Code { get; set; }
    }
    public class License
    {
        public string Type { get; set; }
        public string ID { get; set; }
    }
    public class Validation
    {
        public bool Valid { get; set; }
        public string Detail { get; set; }
        public string code { get; set; }
    }
    public class Machine
    {
        public string Type { get; set; }
        public string ID { get; set; }
    }
    public class LicenseFile
    {
        public string enc { get; set; }
        public string sig { get; set; }
        public string alg { get; set; }
    }
}
