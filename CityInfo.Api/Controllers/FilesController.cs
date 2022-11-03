using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.Api.Controllers {
    /// <summary>
    /// Liefert den Dateiinhalt zu der angegebenen fileId
    /// </summary>
    [Route("api/v{version:apiVersion}/files")]
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    public class FilesController : ControllerBase {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider) {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider ??
                throw new System.ArgumentNullException(nameof(fileExtensionContentTypeProvider));
        }

        [HttpGet("{fileId}")]
        public ActionResult GetFile(string fileId) {
            var fileName = "Calculus Made Easy.pdf";

            if (!System.IO.File.Exists(fileName)) {
                return NotFound();
            }

            if (!_fileExtensionContentTypeProvider.TryGetContentType(fileName, out var contentType)) {
                contentType = "application/octet-stream";
            }

            var bytes = System.IO.File.ReadAllBytes(fileName);
            return File(bytes, contentType, Path.GetFileName(fileName));
        }
    }
}
