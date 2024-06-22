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
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using app_example_net_core.Controllers;

namespace API_Server.Controllers
{

    [Route("api/")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UsersModel UserModel = new UsersModel();
        //private FarmsModel farmModel = new FarmsModel();
        //private DevicesModel devicesModel = new DevicesModel();
        public static Users storedUser { get; private set; }
        
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
        
        [HttpGet("getUsersByEmail/{email}")]
        public IActionResult Get(string email)
        {
            var result = UserModel.GetUserByEmail(email);

            if (UserModel.userByEmail.Id != 0)
            {
                storedUser = UserModel.userByEmail;
                return Ok(storedUser);
            }

            return BadRequest(result);
        }

        //[Route("getAllUserFarms")]
        //[HttpGet]
        //public ActionResult<IEnumerable<Farms>> Post()
        //{
        //    if (storedUser.DataID == 0) return StatusCode(200, "Нет пользователя");

        //    var result = farmModel.GetAllUserFarms(storedUser.Id);
        //    var allUserFarms = farmModel.AllFarms;
        //    return allUserFarms;
        //}

        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    usersList.Remove(usersList.SingleOrDefault(u => u.Id == id));
        //    return Ok(new { Messsage = "Deleted successfully" });
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

            
            return Ok(result);
        }

        [HttpPost("insertUserData")]
        public IActionResult Post(int userID, string phoneNumber)
        {
            var newUserData = new Users();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            newUserData.Id = userID;
            newUserData.PhoneNumber = phoneNumber;

            var result = UserModel.AddUserDataToDB(newUserData);

            return Ok(result);
        }

        //[HttpPost("insertFarms")]
        //public IActionResult Post(int idUser, string ipAddress, string farmAddress)
        //{
        //    var checkNextID = dbModel.Connection("SELECT farm_id FROM db_project.farms ORDER BY farm_id DESC LIMIT 1");
        //    int nextIDFarm = 0;
        //    if (checkNextID)
        //    {
        //        nextIDFarm  = Convert.ToInt32(dbModel.dataTable.Rows[0][0]) + 1;
        //    }
        //    else return StatusCode(500, "не выолнен SQL код для инициализации ID");

        //    var newFarm = new Farms();

        //    newFarm.IPAdress = ipAddress;
        //    newFarm.Id = nextIDFarm;
        //    newFarm.FarmAddress = farmAddress;
        //    var result = farmModel.AddFarms(newFarm, idUser, farmAddress);
        //    return Ok(result);
        //}


        //[HttpPost("insertPlants")]
        //public IActionResult Post(string plantName, int height, DateTime datePlanted, int numberSprouts, int farmID, string variety, string status = null)
        //{
        //    var newPlant = new Plants();

        //    newPlant.Name = plantName;
        //    newPlant.Height = height;
        //    newPlant.DatePlanted = datePlanted;
        //    newPlant.NumberSprouts = numberSprouts;
        //    newPlant.FarmID = farmID;
        //    newPlant.Status = status;
        //    newPlant.Variety = variety;

        //    var isCreate = farmModel.AddPlants(newPlant);
        //    if(isCreate) return StatusCode(200, "Растение добавлено в таблицу");

        //    return StatusCode(500, "Ошибка!");
        //}

        //[HttpPost("insertFertilizer")]
        //public IActionResult Post(string nameFertilizer, int volumeUse, int plantID, DateTime curDate)
        //{
        //    var newFertilizer = new Fertilizers();
        //    newFertilizer.NameFertilizers = nameFertilizer;
        //    newFertilizer.VolumeUser = volumeUse;
        //    newFertilizer.PlantID = plantID;
        //    newFertilizer.Currentdate = curDate;

        //    var result = farmModel.AddFertilizer(newFertilizer);
        //    if (result) return Ok(newFertilizer);
        //    else return StatusCode(500, "Ошибка!");
        //}


        [HttpPut("updateUser")]
        public IActionResult Put(string fname = null, string lname = null, string midname = null, string email = null, string password = null, string phoneNumber = null)
        {
            if(storedUser == null) return StatusCode(400, "Нет пользователся");
            var changesUser = new Users();
            changesUser.Id = storedUser.Id;
            changesUser.FirstName = fname;
            changesUser.LastName = lname;
            changesUser.MiddleName = midname;
            changesUser.Email = email;
            changesUser.Password = password;
            changesUser.PhoneNumber = phoneNumber;
            
            

            var result = UserModel.UpdateUser(changesUser);
            if (result && storedUser.DataID == 0) return Ok("все поля кроме полей дополнительной информации были обновлены");
            if (result) return Ok(changesUser);
            else return StatusCode(500, "Что то пошло не так");

        }

        [HttpDelete("exitUser")]
        public IActionResult Exit()
        {
            storedUser = null;
            return Ok("Дынне пользователя удалены");
        }

        //[HttpPut("updatePlant")]
        //public IActionResult Put()
        //{

        //}

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


    }
}
