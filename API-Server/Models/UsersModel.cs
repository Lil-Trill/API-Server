﻿using System.Diagnostics.Contracts;
using System.Data;
using Npgsql;
using System.ComponentModel.DataAnnotations;
using API_Server.Entities;

namespace API_Server.Models
{
    public class UsersModel
    {
        private static string connectionString = "Server=localhost;Port=5432; Database=farms; User Id = postgres; Password = 1234";
        public static List<Users> AllUsers = new List<Users>();


        public static List<Users> GetAllUsers()
        {
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    if (sqlConnection.State == ConnectionState.Open)
                    {
                        NpgsqlCommand command = new NpgsqlCommand();
                        command.Connection = sqlConnection;
                        command.CommandType = CommandType.Text;
                        command.CommandText = "SELECT * FROM db_project.users";
                        NpgsqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);

                            foreach (DataRow row in dt.Rows)
                            {
                                //foreach (var item in row.ItemArray)
                                //{
                                //    Console.Write(item + "\t"); // Выводим значение каждой ячейки в строке
                                //}
                                //Console.WriteLine(); // Переходим на новую строку после каждой строки
                                AllUsers.Add(
                                        new Users()
                                        {
                                            Id = Convert.ToInt32(row.ItemArray[0]),
                                            FirstName = (string)row.ItemArray[1],
                                            LastName = (string)row.ItemArray[2],
                                            MiddleName = (string)row.ItemArray[3],
                                            Email = (string)row.ItemArray[4],
                                            Password = (string)row.ItemArray[5]
                                        }
                                    );

                            }
                        }
                        command.Dispose();
                    }
                    else
                    {
                        Console.WriteLine("Состояние подключения: " + sqlConnection.State);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return AllUsers;
        }

        //public static string WriteUserToDB(Users newUser)
        //{

        //}
    }
}
