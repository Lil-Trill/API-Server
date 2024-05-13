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

namespace API_Server.Controllers
{

    [Route("api/users")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private UsersModel UserModel = new UsersModel();
        private FarmsModel farmModel = new FarmsModel();
        private static Users storedUser = new Users();
        
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
            var result = UserModel.GetUserByEmail(email);

            if (UserModel.userByEmail.Id != 0)
            {
                storedUser = UserModel.userByEmail;
                return Ok(storedUser);
            }

            storedUser.Id = 0;
            return BadRequest(result);
        }



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
        public IActionResult Post(string farmAddress, int userID, string phoneNumber)
        {
            var newUserData = new Users();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            newUserData.FarmAddress = farmAddress;
            newUserData.Id = userID;
            newUserData.PhoneNumber = phoneNumber;

            var result = UserModel.AddUserDataToDB(newUserData);

            return Ok(result);
        }

        [HttpPost("insertFarms")]
        public IActionResult Post(int idUser, string ipAddress)
        {
            var checkNextID = dbModel.Connection("SELECT farm_id FROM db_project.farms ORDER BY farm_id DESC LIMIT 1");
            int nextIDFarm = 0;
            if (checkNextID)
            {
                nextIDFarm  = Convert.ToInt32(dbModel.dataTable.Rows[0][0]) + 1;
            }
            else return StatusCode(500, "не выолнен SQL код для инициализации ID");

            var newFarm = new Farms();

            newFarm.IPAdress = ipAddress;
            newFarm.Id = nextIDFarm;
            var result = farmModel.AddFarms(newFarm, idUser);
            return Ok(result);
        }


        [HttpPost("insertPlants")]
        public IActionResult Post(string plantName, int height, DateTime datePlanted, int numberSprouts, int farmID, string variety, string status = null)
        {
            var newPlant = new Plants();

            newPlant.Name = plantName;
            newPlant.Height = height;
            newPlant.DatePlanted = datePlanted;
            newPlant.NumberSprouts = numberSprouts;
            newPlant.FarmID = farmID;
            newPlant.Status = status;
            newPlant.Variety = variety;

            var isCreate = farmModel.AddPlants(newPlant);
            if(isCreate) return StatusCode(200, "Растение добавлено в таблицу");

            return StatusCode(500, "Ошибка!");
        }

        [HttpPost("insertFertilizer")]
        public IActionResult Post(string nameFertilizer, int volumeUse, int plantID, DateTime curDate)
        {
            var newFertilizer = new Fertilizers();
            newFertilizer.NameFertilizers = nameFertilizer;
            newFertilizer.VolumeUser = volumeUse;
            newFertilizer.PlantID = plantID;
            newFertilizer.Currentdate = curDate;

            var result = farmModel.AddFertilizer(newFertilizer);
            if (result) return Ok(newFertilizer);
            else return StatusCode(500, "Ошибка!");
        }


        [HttpPut("updateUser")]
        public IActionResult Put(string fname = null, string lname = null, string midname = null, string email = null, string password = null, string farmAddress = null, string phoneNumber = null)
        {
            if(storedUser.Id == 0) return StatusCode(400, "Нет пользователся");
            var changesUser = new Users();
            changesUser.Id = storedUser.Id;
            changesUser.FirstName = fname;
            changesUser.LastName = lname;
            changesUser.MiddleName = midname;
            changesUser.Email = email;
            changesUser.Password = password;
            changesUser.FarmAddress = farmAddress;
            changesUser.PhoneNumber = phoneNumber;

            var result = UserModel.EditUser(changesUser);
            if(result) return Ok(changesUser);
            else return StatusCode(400, "Что то пошло не так");

        }

       

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
