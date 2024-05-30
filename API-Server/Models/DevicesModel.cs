using app_example_net_core.Entities;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using API_Server.Controllers;
using API_Server.Entities;
using System.Runtime.InteropServices;
using System.Data;

namespace app_example_net_core.Models
{
    public class DevicesModel
    {
        DBModel db = new DBModel();
        public List<Devices> AllDevices = new List<Devices>();
        public string GetAllUserDevices(int userID, int farmID)
        {
            string query = "SELECT device_id, db_project.devices.ip_address, db_project.devices.farm_id\r\nFROM db_project.devices\r\nJOIN db_project.farms USING(farm_id)\r\nJOIN db_project.users_farms USING(farm_id)\r\nJOIN db_project.users USING(user_id)\r\nWHERE db_project.users.user_id = 1\r\nORDER BY device_id ASC \r\n";

            var request = db.Connection(query);

            var dataTable = db.dataTable;

            if (dataTable != null)
            {
                foreach (DataRow device in dataTable.Rows)
                {
                    AllDevices.Add(
                        new Devices()
                        {
                            ID = Convert.ToInt32(device.ItemArray[0]),
                            IP = Convert.ToString(device.ItemArray[1]),
                            FarmID = Convert.ToInt32(device.ItemArray[3])
                        }
                      );

                }
            }
            else if (dataTable == null) return "Данный пользователь не имеет устройств на ферме.";

            if (request) return "запрос выполнен успешно";
            else return "Ошибка запроса";
        }
        public void GetDataSensorTempearture(int userID, int deviceID)
        {
            var temperature = new Sensors();
            var humaditu = new Sensors();
        }
    }
}
