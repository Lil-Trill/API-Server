using app_example_net_core.Entities;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using API_Server.Controllers;
using API_Server.Entities;
using System.Runtime.InteropServices;
using System.Data;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System.Net;

namespace app_example_net_core.Models
{
    public class TemperatureHumadity
    {
        public double Temperature { get; set; }
        public double Humidity { get; set; }
    }

    public class DevicesModel
    {
        DBModel db = new DBModel();
        public List<Devices> AllDevices = new List<Devices>();
        public List<Sensors> AllSensors = new List<Sensors>();
        private readonly HttpClient client = new HttpClient();
        private SensorsData temperatureSensor = new SensorsData();
        private SensorsData humaditySensor = new SensorsData();
        public List<SensorsData> ListData = new List<SensorsData> ();

        public string GetAllFarmDevices(int userID, int farmID)
        {
            string query = $"SELECT device_id, db_project.devices.ip_address, db_project.devices.farm_id\r\nFROM db_project.devices\r\nJOIN db_project.farms USING(farm_id)\r\nJOIN db_project.users_farms USING(farm_id)\r\nJOIN db_project.users USING(user_id)\r\nWHERE user_id = {userID} AND db_project.farms.farm_id = {farmID}\r\nORDER BY device_id ASC ";

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
                            FarmID = Convert.ToInt32(device.ItemArray[2])
                        }
                      );

                }
            }
            else if (dataTable == null) return "Данный пользователь не имеет устройств на ферме.";

            if (request) return "запрос выполнен успешно";
            else return "Ошибка запроса";
        }

        public string GetAllDeviceSensors(int userID, int farmID, int deviceID)
        {
            string query = $"SELECT sensor_id, name_type, device_id\r\nFROM db_project.sensors\r\nJOIN db_project.devices USING (device_id)\r\nJOIN db_project.types_sensors USING(type_id)\r\nJOIN db_project.farms USING(farm_id)\r\nJOIN db_project.users_farms USING(farm_id)\r\nJOIN db_project.users USING(user_id)\r\nWHERE user_id = {userID} AND device_id = {deviceID} AND farm_id = {farmID}\r\nORDER BY sensor_id ASC ";

            var request = db.Connection(query);

            if(db.dataTable != null)
            {
                foreach (DataRow sensor in db.dataTable.Rows)
                {
                    AllSensors.Add(
                        new Sensors()
                        {
                            ID = Convert.ToInt32(sensor.ItemArray[0]),
                            NameType = Convert.ToString(sensor.ItemArray[1]),
                            DeviceID = Convert.ToInt32(sensor.ItemArray[2])
                        }
                      );

                }
                
            }

            if(request) return "Запрос выполнен успешно";
            return "Произошла ошибка";
        }



        public string GetTemperatureHumadity(string url, int sensorID)
        {
            var data = GetSensorTemperatureHumadityData(url);

            if (data != null)
            {
                temperatureSensor.Name = "Температура";
                temperatureSensor.Value = data.Temperature;
                humaditySensor.Name = "Влажность";
                humaditySensor.Value = data.Humidity;

                ListData.Add(temperatureSensor);
                ListData.Add(humaditySensor);
            }
            else return "Данные не были получены.";

            double val = temperatureSensor.Value;
            string name = temperatureSensor.Name;
            int id = sensorID;

            string queryTemperature = $"UPDATE db_project.sensors_data SET value = '{Convert.ToInt32(temperatureSensor.Value)}'::real WHERE name = '{temperatureSensor.Name}' AND sensor_id = 1;";
            string queryHumadity = $"UPDATE db_project.sensors_data SET\r\n value = {Convert.ToInt32(humaditySensor.Value)}::real WHERE\r\nname = '{humaditySensor.Name}' AND sensor_id = {sensorID};";

            var requestTemperature = db.Connection(queryTemperature);
            var requestHumadityt = db.Connection(queryHumadity);

            if (requestTemperature || requestHumadityt) return "Данные отправлены на сервер";
            else return "что то пошло не так";

        }

        private TemperatureHumadity GetSensorTemperatureHumadityData(string address)
        {
            string url = $"http://{address}/data";
            try
            {
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();

                string responseBody = response.Content.ReadAsStringAsync().Result;

                TemperatureHumadity data = JsonConvert.DeserializeObject<TemperatureHumadity>(responseBody);
                return data;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
                return null;
            }
        }
    }
}
