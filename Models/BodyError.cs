using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebToPdf.Models
{
    public class BodyError
    {
        public bool Error { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}