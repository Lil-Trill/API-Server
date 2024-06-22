using API_Server.Controllers;
using API_Server.Entities;
using app_example_net_core.Entities;
using app_example_net_core.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace app_example_net_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        DevicesModel devicesModel = new DevicesModel();

        [HttpGet("allFarmDevices")]
        public ActionResult<IEnumerable<Devices>> Get(int farmID)
        {
            if (UsersController.storedUser == null) return StatusCode(200, "Нет пользователя");

            devicesModel.GetAllFarmDevices(UsersController.storedUser.Id,farmID);
            return Ok(devicesModel.AllDevices);
        }

        [HttpGet("allDeviceSensors")]
        public ActionResult<IEnumerable<Sensors>> Get(int farmID, int deviceID)
        {
            if (UsersController.storedUser == null) return StatusCode(200, "Нет пользователя");

            devicesModel.GetAllDeviceSensors(UsersController.storedUser.Id, farmID, deviceID);
            return Ok(devicesModel.AllSensors);
        }

        [HttpGet("getCurrentDataSensor")]
        public ActionResult<IEnumerable<SensorsData>> Get(string url, int sensorID)
        {
            var result = devicesModel.GetTemperatureHumadity(url, sensorID);

            //return Ok(result);
            if (devicesModel.ListData == null) return Ok("Данные не получены");

            return Ok(devicesModel.ListData);
        }
    }
}
