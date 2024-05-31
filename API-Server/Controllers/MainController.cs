using API_Server.Entities;
using API_Server.Models;
using app_example_net_core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace app_example_net_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private UsersModel UserModel = new UsersModel();
        private FarmsModel farmModel = new FarmsModel();
        private DevicesModel devicesModel = new DevicesModel();
        private static Users storedUser = new Users();

        //public static List<Users> usersList = UsersModel.GetAllUsers();
        DBModel dbModel = new DBModel();
    }
}
