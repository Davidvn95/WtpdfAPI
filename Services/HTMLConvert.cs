using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using SelectPdf;
using WebToPdf.Models;

namespace WebToPdf.Services
{
    public class HTMLConvert

    {
        static public byte[] CreateCompress(CreateZipRequest body)
        {
            byte[] pdfDocument = CreatePdfFromGroup(body.Urls);

            using (var stream = new MemoryStream(pdfDocument))

            using (var zipStrem = new MemoryStream())
            {
                using (var archive = new ZipArchive(zipStrem, ZipArchiveMode.Create, true))
                {
                    var zipEntry = archive.CreateEntry($"{body.Name}_document.pdf", CompressionLevel.Fastest);
                    using var entryStream = zipEntry.Open();
                    stream.CopyTo(entryStream);
                }

                zipStrem.Position = 0;
                byte[] zipDoc = zipStrem.ToArray();
                zipStrem.Close();

                return zipDoc;
            };
        }

        static public byte[] CreatePdf(string url)
        {
            HtmlToPdf converter = new();
            PdfDocument doc = converter.ConvertUrl(url);

            using var stream = new MemoryStream();
            doc.Save(stream);
            doc.Close();
            stream.Position = 0;

            byte[] pdfDoc = stream.ToArray();
            stream.Close();
            return pdfDoc;
        }

        static public byte[] CreatePdfFromGroup(string[] urls)
        {
            HtmlToPdf converter = new();
            PdfDocument finalDoc = new();

            foreach (var url in urls)
            {
                PdfDocument doc = converter.ConvertUrl(url);

                for (int i = 0; i < doc.Pages.Count; i++)
                {
                    finalDoc.AddPage(doc.Pages[i]);
                };
            };

            using var stream = new MemoryStream();
            finalDoc.Save(stream);
            finalDoc.Close();
            stream.Position = 0;

            byte[] pdfDoc = stream.ToArray();
            stream.Close();

            return pdfDoc;
        }
    }
}