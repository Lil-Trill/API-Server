namespace app_example_net_core.Entities
{
    public class PlantHistory : Plants
    {
        public string OldStatus { get; set; }
        public DateTime? CurDate { get; set; }
        public string NewStatus { get; set; }
    }
}
