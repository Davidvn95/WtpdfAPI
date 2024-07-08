using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebToPdf.Models
{
    public class PdfRequest
    {
        public required string Url { get; set;}
    }

    public class PdfResponseList
    {
        public required string[] Urls { get; set;}
    }
}