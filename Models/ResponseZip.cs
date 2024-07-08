using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebToPdf.Models
{
    public class AllZipResponse
    {
        public bool IsResult { get; set; } = true;
        public ZipResponse[]? Data { get; set; }
    }

    public class CreatedZipResponse
    {
        public bool IsCreated { get; set; } = true;
        public string? UrlZip {get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class ZipResponse
    {
        public int? Id { get; set; }
        public string Name { get; set; } = "";
        public string Link { get; set; } = "";
    }
}