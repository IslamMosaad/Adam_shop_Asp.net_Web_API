using EcommerceAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.IO;
namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {


        [HttpPost]
        public ActionResult Upload()
        {
            try
            {
                IFormFile file = Request.Form.Files[0];
                string folderName = Path.Combine("wwwroot", "Uploaded", "Images");
                string basePath = Directory.GetCurrentDirectory(); //Path.Combine(configuration.GetValue<string>("imgSavePath"), imgName);
                string pathToSave = Path.Combine(basePath, folderName);
                if (file.Length > 0)
                {
                    string fileName = Guid.NewGuid() + "__" + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName?.Trim('"');
                    string fullPath = Path.Combine(pathToSave, fileName);
                    // string dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { imageRelativePath = fileName });//dbPath
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest();


        }

        [HttpGet("{imageName}")]
        public ActionResult GetImage(string imageName)
        {
            string folderName = Path.Combine("wwwroot", "Uploaded", "Images");
            string basePath = Directory.GetCurrentDirectory();

            string fileAbsolutePath = Path.Combine(basePath, folderName, imageName);
            if (System.IO.File.Exists(fileAbsolutePath))
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(fileAbsolutePath);
                string contentType = GetContentType(imageName);
                if (contentType != null)
                {
                    return File(imageBytes, contentType);
                }
            }

            return NotFound();
        }


        private string GetContentType(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLowerInvariant();
            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                // Add more cases for other image formats if needed
                default:
                    return null; // Unsupported file type
            }

        }
    }
}
