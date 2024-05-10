using API_Server.Entities;
using API_Server.Models;
using app_example_net_core.Entities;
using app_example_net_core.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace API_Server.Controllers
{

    [Route("api/users")]
    [ApiController]
    public class APIController : ControllerBase
    {
        UsersModel UserModel = new UsersModel();
        FarmsModel farmModel = new FarmsModel();
        //public static List<Users> usersList = UsersModel.GetAllUsers();
        DBModel dbModel = new DBModel();

        //тут осуществляется запрос для получения всех пользователей из БД
        [Route("allUsers")]
        [HttpGet]
        public IEnumerable<Users> Get()
        {
            UserModel.GetAllUsers();
            return UserModel.AllUsers;
        }

        //тут осуществляется получения пользователя по IP
        
        [HttpGet("getUsersByEmail{email}")]
        public IActionResult Get(string email)
        {
            bool result = UserModel.GetUserByEmail(email);
            if (result)
            {
                return Ok(UserModel.userByEmail);
            }
            return BadRequest();
        }



        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    usersList.Remove(usersList.SingleOrDefault(u => u.Id == id));
        //    return Ok(new { Messsage = "Deleted successfully" });
        //}

        ////Индетификатор
        //private int NextUserId => usersList.Count() == 0 ? 1 : usersList.Max(x => x.Id) + 1;

        //[HttpGet("GetNextUserId")]
        //public int GetNextUserId()
        //{

        //    return NextUserId;
        //}

        ////добавление пользователя

        //[HttpPost("AddUser")]
        //public IActionResult Post(Users user)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    user.Id = NextUserId;
        //    usersList.Add(user);

        //    return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        //}

        [HttpPost("registrationUser")]
        public IActionResult Post(string firstName, string lastName, string midName, string email, string password)
        {
            var newUser = new Users();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            newUser.FirstName = firstName;
            newUser.LastName = lastName;
            newUser.MiddleName = midName;
            newUser.Email = email;
            newUser.Password = password;

            var result = UserModel.AddUserToDB(newUser);

            //var result = UsersModel.AddUserToDB(newUser);

            //if(result)
            //{
            //    return Ok("user added");
            //}
            //else 
            return Ok(result);
        }

        [HttpPost("insertUserData")]
        public IActionResult Post(string farmAddress, int userID, string phoneNumber)
        {
            var newUserData = new UsersData();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            newUserData.FarmAddress = farmAddress;
            newUserData.UserID = userID;
            newUserData.PhoneNumber = phoneNumber;

            var result = UserModel.AddUserDataToDB(newUserData);

            //var result = UsersModel.AddUserToDB(newUser);

            //if(result)
            //{
            //    return Ok("user added");
            //}
            //else 
            return Ok(result);
        }

        //[HttpPost("insertFarms")]
        //public IActionResult Post(int idUser,string ipAddress)
        //{
        //    var checkNextID = dbModel.Connection("SELECT farm_id FROM db_project.farms ORDER BY farm_id DESC LIMIT 1");

        //    var newFarm = new Farms();
            
        //    newFarm.IPAdress = ipAddress;
        //    newFarm.Id = nextIdFarm;
        //    var result = farmModel.AddFarms(newFarm,idUser);
        //    return Ok(nextIdFarm);
        //}


        //[HttpPost]
        //public IActionResult PostBody([FromBody] Users user) => Post(user);


        ////Изменение полей пользователя
        //[HttpPut]
        //public IActionResult Put(Users user)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var storedUser = usersList.SingleOrDefault(u => u.Id == user.Id);

        //    if (storedUser == null) return NotFound();
        //    storedUser.FirstName = user.FirstName;
        //    storedUser.LastName = user.LastName;
        //    //storedUser.MiddleName = user.MiddleName;
        //    storedUser.Email = user.Email;
        //    storedUser.Password = user.Password;
        //    return Ok(storedUser);
        //}


        //[HttpPost("arduino")]
        //public IActionResult Post(float temperature, float humidity)
        //{


        //    return Ok();
        //}

        //[Route("allUsers")]
        //[HttpGet]
        //public ActionResult<IEnumerable<Users>> Get()
        //{
        //    users.GetAllUsers();

        //    return Ok(users.AllUsers);
        //}
    }
}
