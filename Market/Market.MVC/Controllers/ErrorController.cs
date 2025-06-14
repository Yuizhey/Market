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
            if (statusCode == 404)
            {
                return View("Error404");
            }
            else if (statusCode == 403)
            {
                return View("Error403");
            }
            
            return View("Error404"); // По умолчанию показываем 404
        }
    }
} 