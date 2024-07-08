using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebToPdf.Models;
using WebToPdf.Services;

namespace WebToPdf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdfController : Controller
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type=typeof(byte[]))]
        public async Task<IActionResult> CreatePdf(PdfRequest body)
        {
            try
            {
                if (body == null)
                throw new Exception("body request is missing");

                if(string.IsNullOrEmpty(body.Url))
                throw new Exception("Url is missing");
                
                byte[] pdfDocument = HTMLConvert.CreatePdf(body.Url);

                return File(pdfDocument, "application/pdf", "document.pdf");
            }
            catch (Exception ex)
            {
                BodyError err = new()
                {
                    Message = ex.Message,
                };
                return new BadRequestObjectResult(err);
            }
        }
    }
}