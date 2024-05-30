using API_Server.Entities;
using API_Server.Models;
using app_example_net_core.Entities;
using app_example_net_core.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace app_example_net_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmsController : ControllerBase
    {
        private UsersModel UserModel = new UsersModel();
        private FarmsModel farmModel = new FarmsModel();
        private DevicesModel devicesModel = new DevicesModel();
        private static Users storedUser = new Users();

        //public static List<Users> usersList = UsersModel.GetAllUsers();
        DBModel dbModel = new DBModel();

        [Route("getAllUserFarms")]
        [HttpGet]
        public ActionResult<IEnumerable<Farms>> Get()
        {
            if (storedUser.DataID == 0) return StatusCode(200, "Нет пользователя");

            var result = farmModel.GetAllUserFarms(storedUser.Id);
            var allUserFarms = farmModel.AllFarms;
            return allUserFarms;
        }
    }
}
