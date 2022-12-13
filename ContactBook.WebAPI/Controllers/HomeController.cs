using Microsoft.AspNetCore.Mvc;

namespace ContactBook.WebAPI.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Gets Swagger UI.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /
        /// </remarks>
        /// <response code="200">Returns "OK" with API info.</response>
        [HttpGet]
        [Route("/")]
        [Route("/api")]
        [Route("/swagger")]
        public IActionResult Index() 
            => LocalRedirect(@"/swagger/index.html");
    }
}
