using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebToPdf.Models
{
    public class CreateZipRequest
    {
        [Required (ErrorMessage = "El Nombre para el archivo es requerido")]
        public required string Name { get; set; }
        [Required (ErrorMessage = "Las urls son requeridas")]
        public required string[] Urls { get; set; }
    }

    public class CreateZipDB
    {
        public required string Name { get; set; }
        public required string Content { get; set;}
    }
}