using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectPdf;
using WebToPdf.Database;
using WebToPdf.Models;
using WebToPdf.Services;

namespace WebToPdf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZipController : Controller
    {
        private readonly ApplicationDBContext _zipDBContext;
        public ZipController(ApplicationDBContext zipDBContext)
        {
            _zipDBContext = zipDBContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ZipResponse[]))]
        public async Task<IActionResult> GetZips()
        {
            try
            {
                ZipModel[] response = await _zipDBContext.GetAll();

                List<ZipResponse> result = [];
                foreach (var zip in response)
                {
                    ZipResponse newZip = new()
                    {
                        Id = zip.Id,
                        Name = zip.Name,
                        Link = Url.Action(nameof(GetZip), "zip", new { id = zip.Id }, Request.Scheme),
                    };
                    result.Add(newZip);
                }

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                BodyError err = new()
                {
                    Error = true,
                    Message = ex.Message,
                };
                return new BadRequestObjectResult(err);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(byte[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type= typeof(BodyError))]
        public async Task<IActionResult> GetZip(int id)
        {
            try
            {
                var result = await _zipDBContext.Get(id);
                if (result == null)
                {
                    BodyError err = new()
                    {
                        Message = $"El documento con Id {id} no existe"
                    };

                    return new NotFoundObjectResult(err);
                }

                byte[] content = Convert.FromBase64String(result.Content);
                using var zipStream = new MemoryStream(content);
                zipStream.Position = 0;
                return File(zipStream.ToArray(), "application/zip", "document_zip.zip");
            }
            catch (Exception ex)
            {
                BodyError err = new()
                    {
                    Message = ex.Message
                };

                return new NotFoundObjectResult(err);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(byte[]))]
        public async Task<IActionResult> Post(CreateZipRequest zipBody)
        {
            try
            {
                BodyError err = BodyValidator.UrlList(zipBody.Urls);
                if (err.Error == true)
                    return new BadRequestObjectResult(err);

                byte[] zipDocument = HTMLConvert.CreateCompress(zipBody);
                string contentdb = Convert.ToBase64String(zipDocument);

                CreateZipDB bodyDB = new()
                {
                    Content = contentdb,
                    Name = zipBody.Name,
                };

                await _zipDBContext.Add(bodyDB);

                return File(zipDocument, "application/zip", $"{zipBody.Name}_doc.zip");
            }
            catch (Exception ex)
            {
                BodyError err = new()
                {
                    Message = ex.Message
                };
                return new BadRequestObjectResult(err);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public async Task<IActionResult> DeleteZip(int id)
        {
            try
            {
                var result = await _zipDBContext.Delete(id);
                if(result == false)
                {
                    BodyError err = new()
                    {
                        Message = $"El documento con Id {id} no existe"
                    };

                    return new NotFoundObjectResult(err);
                }
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                BodyError err = new()
                {
                    Message = ex.Message
                };
                return new BadRequestObjectResult(err);
            }
        }
    }
}