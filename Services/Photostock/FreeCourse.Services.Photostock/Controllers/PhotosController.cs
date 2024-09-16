using FreeCourse.Services.Photostock.DTOs;
using FreeCourse.Shared.BaseController;
using FreeCourse.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Photostock.Controllers
{
    public class PhotosController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken)
        {
            if (photo != null && photo.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);

                using var stream = new FileStream(path, FileMode.Create);
                await photo.CopyToAsync(stream, cancellationToken);
                var returnPath = "photos/" + photo.FileName;

                PhotoDTO photoDTO = new() { Url = returnPath };
                return CreateActionResult(ResponseDTO<PhotoDTO>.Success(photoDTO, 200));
            }

            return CreateActionResult(ResponseDTO<PhotoDTO>.Fail("Photo could not be saved!", 400));
        }

        [HttpDelete]
        public IActionResult PhotoDelete(string photoUrl)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoUrl);

            if (!System.IO.File.Exists(path)) return CreateActionResult(ResponseDTO<NoContentDTO>.Fail("Photo not found!", 404));

            System.IO.File.Delete(path);

            return CreateActionResult(ResponseDTO<NoContentDTO>.Success(204));
        }
    }
}
