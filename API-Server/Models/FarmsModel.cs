using app_example_net_core.Entities;

namespace app_example_net_core.Models
{


    public class FarmsModel
    {
        DBModel dbModel = new DBModel();


        public string AddFarms(Farms newFarm, int idUser)
        {
            var isCreateFarms = dbModel.Connection($"INSERT INTO db_project.farms (farm_id,ip_address) VALUES ({newFarm.Id}::bigint,'{newFarm.IPAdress}'::text);");

            var isCreateFarmsUsers = dbModel.Connection($"INSERT INTO db_project.users_farms (farm_id, user_id) VALUES ('{idUser}'::bigint, '{newFarm.Id}'::bigint returning users_farms_id;");

            if (isCreateFarms && isCreateFarmsUsers) return "Ферма добавлена в БД!!!";
            return "Произошла ошибка :(";
        }
    }
}
