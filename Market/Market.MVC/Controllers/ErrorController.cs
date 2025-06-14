using Microsoft.AspNetCore.Mvc;

namespace Market.MVC.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/404")]
        public IActionResult Error404()
        {
            return View();
        }

        [Route("Error/403")]
        public IActionResult Error403()
        {
            return View();
        }

        // Добавляем общий обработчик для всех кодов ошибок
        [Route("Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            return statusCode switch
            {
                400 => View("Error400"),
                401 => View("Error401"),
                403 => View("Error403"),
                404 => View("Error404"),
                500 => View("Error500"),
                503 => View("Error503"),
                _ => View("Error404")
            };
        }
    }
} 