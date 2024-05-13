using app_example_net_core.Entities;

namespace app_example_net_core.Models
{


    public class FarmsModel
    {
        DBModel dbModel = new DBModel();


        public string AddFarms(Farms newFarm, int idUser)
        {
            var checkUser = dbModel.Connection($"SELECT * \r\nFROM db_project.users\r\nWHERE user_id = {idUser}");
            var table = dbModel.dataTable;
            if (table.Rows.Count != 0)
            {
                var isCreateFarms = dbModel.Connection($"INSERT INTO db_project.farms (farm_id,ip_address) VALUES ({newFarm.Id}::bigint,'{newFarm.IPAdress}'::text);");

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
