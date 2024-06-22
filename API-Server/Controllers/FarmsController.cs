using API_Server.Controllers;
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
        private FarmsModel farmModel = new FarmsModel();
        

        //public static List<Users> usersList = UsersModel.GetAllUsers();
        DBModel dbModel = new DBModel();



        [Route("getAllUserFarms")]
        [HttpGet]
        public ActionResult<IEnumerable<Farms>> Get()
        {
            if (UsersController.storedUser == null) return StatusCode(200, "Нет пользователя");

            var result = farmModel.GetAllUserFarms(UsersController.storedUser.Id);
            var allUserFarms = farmModel.AllFarms;
            return Ok(allUserFarms);
        }

        [Route("getPlantsFromUsersFarm")]
        [HttpGet]
        public ActionResult<IEnumerable<Plants>> GetPlants(int farmID)
        {
            if (UsersController.storedUser == null) return StatusCode(200, "Нет пользователя");

            var result = farmModel.GetAllPlantsFromUsersFarm(farmID, UsersController.storedUser.Id);
            var allPlants = farmModel.AllPlants;
            return Ok(farmModel.AllPlants);
        }


        [HttpPost("insertFarms")]
        public IActionResult Post(int idUser, string ipAddress, string farmAddress)
        {
            var checkNextID = dbModel.Connection("SELECT farm_id FROM db_project.farms ORDER BY farm_id DESC LIMIT 1");
            int nextIDFarm = 0;
            if (checkNextID)
            {
                nextIDFarm = Convert.ToInt32(dbModel.dataTable.Rows[0][0]) + 1;
            }
            else return StatusCode(500, "не выолнен SQL код для инициализации ID");

            var newFarm = new Farms();

            newFarm.IPAdress = ipAddress;
            newFarm.Id = nextIDFarm;
            newFarm.FarmAddress = farmAddress;
            var result = farmModel.AddFarms(newFarm, idUser, farmAddress);
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
            if (isCreate) return StatusCode(200, "Растение добавлено в таблицу");

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

        [HttpPut("updatePlant")]
        public IActionResult Put(int? plantID, string plantName = null, int? height = 0, int? numberSprout = 0, string status = null)
        {
            var changesPlant = new Plants();

            changesPlant.ID = plantID;
            changesPlant.Name = plantName;
            changesPlant.Height = height;
            changesPlant.NumberSprouts = numberSprout;
            changesPlant.Status = status;

            return Ok(farmModel.UpdatePlant(changesPlant));
        }
        
        //[HttpPut("updatePlants")]
        //public IActionResult Put()
        //{

        //}

    }
}
