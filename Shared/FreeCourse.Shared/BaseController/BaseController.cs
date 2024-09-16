using FreeCourse.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Shared.BaseController
{

    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public IActionResult CreateActionResult<T>(ResponseDTO<T> responseDTO)
        {
            return new ObjectResult(responseDTO) { StatusCode = responseDTO.StatusCode };
        }
    }
}
