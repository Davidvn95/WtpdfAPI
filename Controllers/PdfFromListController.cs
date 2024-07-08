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
    [Route("api/[controller]")]
    public class PdfFromListController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> CreatePdfFromList(PdfResponseList body)
        {
            try
            {
                BodyError err = BodyValidator.UrlList(body.Urls);
                if(err.Error == true)
                return new BadRequestObjectResult(err);

                byte[] pdfDocument = HTMLConvert.CreatePdfFromGroup(body.Urls);

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