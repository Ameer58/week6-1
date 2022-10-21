////Ignore - used to demo lifetime services

//using Microsoft.AspNetCore.Mvc;
//using TodoApi.Services;

//namespace TodoApi.Controllers
//{
//    [ApiController]
//    [Route("nish")]
//    public class WeatherForecastController : ControllerBase
//    {

//        private ILogger<WeatherForecastController> _logger;
//        private readonly ITodoItemService _service;
//        private ILogger<WeatherForecastController> _logger2;
//        private readonly ITodoItemService _service2;

//        public WeatherForecastController(ILogger<WeatherForecastController> logger, ITodoItemService service, ITodoItemService service2)
//        {
//            _logger = logger;
//            _service = service;
//            _service2 = service2;
//        }
//        [HttpGet]
//        public string Get()
//        {
//            var result = $" service: {_service.GetHashCode()}" +
//                $"\n service2: {_service2.GetHashCode()}" +
//                $"\n logger: {_logger.GetHashCode()} ";
//            return result;

//        }

//    }
//}