//using API_Server.Entities;
//using API_Server.Models;
//using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Npgsql;
//using System.Data;

//namespace API_Server.Controllers
//{

//    [Route("api/users")]
//    [ApiController]
//    public class UserController : ControllerBase
//    {
//        public static List<Users> usersList = UsersModel.GetAllUsers();

//        //тут осуществляется запрос для получения всех пользователей из БД
//        [Route("allUsers")]
//        [HttpGet]
//        public IEnumerable<Users> Get()
//        {
//            return usersList;
//        }

//        //тут осуществляется получения пользователя по IP
//        [HttpGet("{id}")]
//        public IActionResult Get(int id)
//        {
//            var user = usersList.SingleOrDefault(u => u.Id == id);

//            if (user == null)
//            {
//                return NotFound();
//            }
//            return Ok(user);
//        }

//        [HttpDelete("{id}")]
//        public IActionResult Delete(int id)
//        {
//            usersList.Remove(usersList.SingleOrDefault(u => u.Id == id));
//            return Ok(new {Messsage = "Deleted successfully"});
//        }

//        //Индетификатор
//        private int NextUserId => usersList.Count() == 0 ? 1 : usersList.Max(x => x.Id) + 1;

//        [HttpGet("GetNextUserId")]
//        public int GetNextUserId()
//        {
//            return NextUserId;
//        }

//        //добавление пользователя

//        [HttpPost]
//        public IActionResult Post(Users user)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            user.Id = NextUserId;
//            usersList.Add(user);

//            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
//        }

//        [HttpPost("AddUser")]
//        public IActionResult PostBody([FromBody] Users user) => Post(user);


//        //Изменение полей пользователя
//        [HttpPut]
//        public IActionResult Put(Users user)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }
//            var storedUser = usersList.SingleOrDefault(u => u.Id == user.Id);

//            if (storedUser == null) return NotFound();
//            storedUser.UserName = user.UserName;
//            storedUser.Email = user.Email;
//            storedUser.Password = user.Password;
//            return Ok(storedUser);
//        }


//        [HttpPost("arduino")]
//        public IActionResult Post( float temperature, float humidity)
//        {


//            return Ok();
//        }

//        //[Route("allUsers")]
//        //[HttpGet]
//        //public ActionResult<IEnumerable<Users>> Get()
//        //{
//        //    users.GetAllUsers();

//        //    return Ok(users.AllUsers);
//        //}
//    }
//}
