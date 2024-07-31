using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ProductivityTools.PDFCommentsExtractor.Api.Controllers
{
    public class PdfController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost("api/upload")]
        public IResult Upload(HttpRequest request)
        {
            if (!request.HasFormContentType || request.Form.Files.Count == 0)
                return Results.BadRequest("No file uploaded");
            var file = request.Form.Files.FirstOrDefault();
            if (file == null || file.Length == 0)
                return Results.BadRequest("File is empty");

            // Happy

            return Results.Ok("TEST");
        }
    }
}
