using System.Diagnostics.Contracts;
using System.Data;
using Npgsql;
using System.ComponentModel.DataAnnotations;
using API_Server.Entities;
using app_example_net_core.Models;
using app_example_net_core.Entities;

namespace API_Server.Models
{
    public class UsersModel
    {
        public List<Users> AllUsers = new List<Users>();
        DBModel db = new DBModel();
        public Users? userByEmail = new Users();
        
        
        public void GetAllUsers()
        {
            string querySelectAll = "SELECT * FROM db_project.users";
            
            var request = db.Connection(querySelectAll);

            var dataTable = db.dataTable;

            if (dataTable != null)
            {
                foreach (DataRow user in dataTable.Rows)
                {
                    AllUsers.Add(
                        new Users()
                        {
                            Id = Convert.ToInt32(user.ItemArray[0]),
                            FirstName = Convert.ToString(user.ItemArray[1]),
                            LastName = Convert.ToString(user.ItemArray[2]),
                            MiddleName = Convert.ToString(user.ItemArray[3]),
                            Email = Convert.ToString(user.ItemArray[4]),
                            Password = Convert.ToString(user.ItemArray[5])
                        }
                      );
                }
            }
        }

        public bool GetUserByEmail(string email)
        {
            string querySelectEmail = $"SELECT * FROM db_project.users WHERE email = '{email}'";
            db.Connection(querySelectEmail);
            var result = db.dataTable;

            if (result != null)
            {
                foreach (DataRow user in result.Rows)
                {
                    userByEmail.Id = Convert.ToInt32(user.ItemArray[0]);
                    userByEmail.FirstName = Convert.ToString(user.ItemArray[1]);
                    userByEmail.LastName = Convert.ToString(user.ItemArray[2]);
                    userByEmail.MiddleName = Convert.ToString(user.ItemArray[3]);
                    userByEmail.Email = Convert.ToString(user.ItemArray[4]);
                    userByEmail.Password = Convert.ToString(user.ItemArray[5]);
                }
                return true;
            }
            return false;
        }



        public string AddUserToDB(Users newUser)
        {
            var request = db.Connection($"INSERT INTO db_project.users (fname, lname, midname, email, password) VALUES ('{newUser.FirstName}'::text, '{newUser.LastName}'::text, '{newUser.MiddleName}'::text, '{newUser.Email}'::text, '{newUser.Password}'::text) returning user_id;");
            if (request)
            {
                return "запрос выполнен успешно, пользователь добавлен";
            }
            else return "произошла ошибка";
        }

        public string AddUserDataToDB(UsersData newUserData)
        {
            var request = db.Connection($"INSERT INTO db_project.users_data (farm_address, user_id,phone_number) VALUES ('{newUserData.FarmAddress}'::text, '{newUserData.UserID}'::bigint, '{newUserData.PhoneNumber}'::text) returning user_data_id;");
            if (request)
            {
                return "запрос выполнен успешно, информация о пользователе добавлена";
            }
            else return "произошла ошибка";
        }
    }
}