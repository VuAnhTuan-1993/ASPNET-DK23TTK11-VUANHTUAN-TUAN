using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace AppleStore_MVC.Controllers
{
    public class ImageController : Controller
    {
        // GET: /image?fname=abc.svg
        public IActionResult Index(string fname)
        {
            if (string.IsNullOrEmpty(fname))
            {
                return NotFound("No filename provided");
            }

            // Đường dẫn vật lý tới file ảnh
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", fname);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found");
            }

            // Xác định content-type dựa vào phần mở rộng
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream"; // fallback
            }

            var imageBytes = System.IO.File.ReadAllBytes(filePath);
            return File(imageBytes, contentType);
        }
    }
}
