using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace app_example_net_core.Entities
{
    public class Plants
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Height { get; set; }
        public DateTime DatePlanted { get; set; }
        public int NumberSprouts {  get; set; }
        public int FarmID { get; set; }
        public string Status { get; set; }
    }
}
