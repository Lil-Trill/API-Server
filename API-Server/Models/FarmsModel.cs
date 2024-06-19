using API_Server.Entities;
using app_example_net_core.Entities;
using System.Data;

namespace app_example_net_core.Models
{


    public class FarmsModel
    {
        DBModel dbModel = new DBModel();
        public List<Farms> AllFarms = new List<Farms>();
        public List<Plants> AllPlants = new List<Plants>();

        public string GetAllUserFarms(int userID)
        {
            string query = $"SELECT farm_id, ip_address, farm_address\r\nFROM db_project.farms\r\nJOIN db_project.users_farms USING(farm_id)\r\nJOIN db_project.users USING(user_id)\r\nWHERE user_id = {userID}\r\nORDER BY farm_id ASC ";

            var request = dbModel.Connection(query);

            var dataTable = dbModel.dataTable;

            if (dataTable != null)
            {
                foreach (DataRow farm in dataTable.Rows)
                {
                    AllFarms.Add(
                        new Farms()
                        {
                            Id = Convert.ToInt32(farm.ItemArray[0]),
                            IPAdress = Convert.ToString(farm.ItemArray[1]),
                            FarmAddress = Convert.ToString(farm.ItemArray[2])
                        }
                      );

                }
            }
            else if (dataTable == null) return "У данного пользователя нет фермы";

            if (request) return "запрос выполнен успешно";
            else return "Ошибка запроса";
        }

        public string GetAllPlantsFromUsersFarm(int farmID, int userID)
        {
            var checkPlants = dbModel.Connection($"SELECT plant_id, plant_name, height, date_planting, number_sprouts, farm_id, status FROM db_project.users JOIN db_project.users_farms USING(user_id) JOIN db_project.farms USING(farm_id) JOIN db_project.plants_data USING(farm_id) WHERE user_id = {userID} AND farm_id = {farmID} ORDER BY user_id ASC");
            var table = dbModel.dataTable;

            if (checkPlants && table.Rows.Count != 0)
            {
                foreach (DataRow plant in table.Rows)
                {
                    AllPlants.Add(
                        new Plants()
                        {
                            ID = Convert.ToInt32(plant.ItemArray[0]),
                            Name = Convert.ToString(plant.ItemArray[1]),
                            Height = Convert.ToInt32(plant.ItemArray[2]),
                            DatePlanted = Convert.ToDateTime(plant.ItemArray[3]),
                            NumberSprouts = Convert.ToInt32(plant.ItemArray[4]),
                            FarmID = Convert.ToInt32(plant.ItemArray[5]),
                            Status = Convert.ToString(plant.ItemArray[6])
                        }
                    );
                }
            }
            else if (table.Rows == null) return "У пользователя нет растений";
            else if (!checkPlants) return "неправильный запрос";

            return "Запрос прошёл успешно";
        }

        public string AddFarms(Farms newFarm, int idUser, string farmAddress)
        {
            var checkUser = dbModel.Connection($"SELECT * \r\nFROM db_project.users\r\nWHERE user_id = {idUser}");
            var table = dbModel.dataTable;
            if (table.Rows.Count != 0)
            {
                var isCreateFarms = dbModel.Connection($"INSERT INTO db_project.farms (farm_id,ip_address,farm_address) VALUES ({newFarm.Id}::bigint,'{newFarm.IPAdress}'::text, '{newFarm.FarmAddress}'::text);");

                var isCreateFarmsUsers = dbModel.Connection($"INSERT INTO db_project.users_farms (farm_id, user_id) VALUES ('{newFarm.Id}'::bigint, '{idUser}'::bigint) returning users_farms_id;");
                if (isCreateFarms && isCreateFarmsUsers) return "Ферма добавлена в БД.";
                else return "Произошла ошибка :(";
            }
            else return "Пользователя с таким ID нет";

           
        }

        public bool AddPlants(Plants newPlant)
        {
            var isCreatePlant = dbModel.Connection($"INSERT INTO db_project.plants_data (plant_name, height, number_sprouts, farm_id) VALUES ('{newPlant.Name}'::text, '{newPlant.Height}'::integer, '{newPlant.NumberSprouts}'::smallint, '{newPlant.FarmID}'::bigint) returning plant_id;");

            return isCreatePlant;
        }

        public bool AddFertilizer(Fertilizers newFertilizer)
        {
            var isCreateFertilizer = dbModel.Connection($"INSERT INTO db_project.fertilizers (\r\nname_fertilize, volume_use, plant_id) VALUES (\r\n'{newFertilizer.NameFertilizers}'::text, '{newFertilizer.VolumeUser}'::real, '{newFertilizer.PlantID}'::bigint)\r\n returning fertilizer_id;");

            return isCreateFertilizer;
        }
    }
}
