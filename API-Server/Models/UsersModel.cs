using System.Diagnostics.Contracts;
using System.Data;
using Npgsql;
using System.ComponentModel.DataAnnotations;
using API_Server.Entities;
using app_example_net_core.Models;

namespace API_Server.Models
{
    public class UsersModel
    {
        public static List<Users> AllUsers = new List<Users>();


        public static List<Users> GetAllUsers()
        {
            string querySelectAll= "SELECT * FROM db_project.users";

            var dateTable = DBModel.Connection(querySelectAll);

            if(dateTable != null)
            {
                foreach(DataRow user in dateTable.Rows) 
                {
                    AllUsers.Add(
                        new Users()
                        {
                            Id = Convert.ToInt32(user.ItemArray[0]),
                            FirstName = (string)user.ItemArray[1],
                            LastName = (string)user.ItemArray[2],
                            MiddleName = (string)user.ItemArray[3],
                            Email = (string)user.ItemArray[4],
                            Password = (string)user.ItemArray[5]
                        }
                      );
                }
            }
            return AllUsers;
        }

        //public static bool AddUserToDB(Users newUser)
        //{
        //    string queryAddUser = $"INSERT INTO db_project.users (fname, lname, midname, email, password) VALUES('{newUser.FirstName}'::text,'{newUser.LastName}'::text,'{newUser.MiddleName}'::text,'{newUser.Email}'::text,'{newUser.Password}'::text) returning user_id;";
        //    var result = DBModel.Connection(queryAddUser);

        //    if(result)

        //    return false;
        //}

        //public static string WriteUserToDB(Users newUser)
        //{

        //}
    }
}
