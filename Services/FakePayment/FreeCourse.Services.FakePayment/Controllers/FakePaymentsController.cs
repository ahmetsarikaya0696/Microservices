using FreeCourse.Shared.BaseController;
using FreeCourse.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.FakePayment.Controllers
{
    public class FakePaymentsController : BaseController
    {

        [HttpPost]
        public IActionResult ReceivePayment()
        {
            return CreateActionResult(ResponseDTO<NoContentDTO>.Success(200));
        }
    }
}
