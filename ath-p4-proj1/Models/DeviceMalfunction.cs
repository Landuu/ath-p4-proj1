
namespace ath_p4_proj1.Models
{
    internal class DeviceMalfunction
    {
        public int DeviceMalfunctionId { get; set; }
        public int DeviceId { get; set; }
        public Device Device { get; set; }
        public string DamageDescription { get; set; }
        public DateTime ReportDate { get; set; }
        public DateTime PlannedServiceDate { get; set; }
        public DateTime RepairDate { get; set; }

    }
}
