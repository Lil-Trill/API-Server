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

        public string GetUserByEmail(string email)
        {
            string querySelectEmail = $"SELECT * FROM db_project.users LEFT JOIN db_project.users_data USING(user_id) WHERE email = '{email}' ORDER BY user_id ASC ";
            var checkConnect = db.Connection(querySelectEmail);
            if (!checkConnect) return "Ошибка на стороне сервера";
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
                    if(user.ItemArray[7] != DBNull.Value) userByEmail.DataID = Convert.ToInt32(user.ItemArray[7]);
                    userByEmail.PhoneNumber = Convert.ToString(user.ItemArray[8]);
                }
                if(userByEmail.Id != 0) return "Пользователь найден";
            }
            return "Пользователь с таким email не найден";
        }


        //public bool CheckFarms(int userID)
        //{
        //    string
        //}
        public string AddUserToDB(Users newUser)
        {
            var request = db.Connection($"INSERT INTO db_project.users (fname, lname, midname, email, password) VALUES ('{newUser.FirstName}'::text, '{newUser.LastName}'::text, '{newUser.MiddleName}'::text, '{newUser.Email}'::text, '{newUser.Password}'::text) returning user_id;");
            if (request)
            {
                return "запрос выполнен успешно, пользователь добавлен";
            }
            else return "произошла ошибка";
        }

        public string AddUserDataToDB(Users newUserData)
        {
            var request = db.Connection($"INSERT INTO db_project.users_data (user_id,phone_number) VALUES ('{newUserData.Id}'::bigint, '{newUserData.PhoneNumber}'::text) returning user_data_id;");
            if (request)
            {
                return "запрос выполнен успешно, информация о пользователе добавлена";
            }
            else return "произошла ошибка";
        }

        public bool UpdateUser(Users changesUser)
        {
            var request = db.Connection($"UPDATE db_project.users SET \r\n fname = CASE WHEN '{changesUser.FirstName}' <> '' THEN '{changesUser.FirstName}' ELSE fname END,\r\n    lname = CASE WHEN '{changesUser.LastName}' <> '' THEN '{changesUser.LastName}' ELSE lname END,\r\n    midname = CASE WHEN '{changesUser.MiddleName}' <> '' THEN '{changesUser.MiddleName}' ELSE midname END,\r\n    email = CASE WHEN '{changesUser.Email}' <> '' THEN '{changesUser.Email}' ELSE email END,\r\n    password = CASE WHEN '{changesUser.Password}' <> '' THEN '{changesUser.Password}' ELSE password END\r\nWHERE user_id = {changesUser.Id};" +
                $"\r\n UPDATE db_project.users_data\r\n SET \r\n phone_number = CASE WHEN '{changesUser.PhoneNumber}' <> '' THEN '{changesUser.PhoneNumber}' ELSE phone_number END\r\n WHERE user_id = {changesUser.Id};");

            return request;
        }

        public bool UpdatePlant(Plants changesPlant)
        {
            return true;
        }
    }
}