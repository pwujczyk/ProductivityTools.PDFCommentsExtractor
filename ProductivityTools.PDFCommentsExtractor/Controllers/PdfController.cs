using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;

namespace ProductivityTools.PDFCommentsExtractor.Api.Controllers
{
    public class PdfController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("api/echo")]
        public string Echo(string hello)
        {
            return hello;
        }

        [HttpPost("api/upload")]
        public IResult Upload(HttpRequest request)
        {

            var body = Request.Body;

            //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
            var buffer = new byte[Convert.ToInt32(Request.ContentLength)];

            //...Then we copy the entire request stream into the new buffer.
            Request.Body.ReadAsync(buffer, 0, buffer.Length);

            //We convert the byte[] into a string using UTF8 encoding...
            var bodyAsText = Encoding.UTF8.GetString(buffer);

            //..and finally, assign the read body back to the request body, which is allowed because of EnableRewind()
            Request.Body.Seek(0, SeekOrigin.Begin);
            Request.Body = body;
            if (!Request.HasFormContentType || Request.Form.Files.Count == 0)
                return Results.BadRequest("No file uploaded");
            var file = Request.Form.Files.FirstOrDefault();
            if (file == null || file.Length == 0)
                return Results.BadRequest("File is empty");

            // Happy

            return Results.Ok("TEST");
        }
    }
}
