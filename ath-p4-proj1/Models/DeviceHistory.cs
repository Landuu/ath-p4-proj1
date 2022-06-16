
namespace ath_p4_proj1.Models
{
    internal class DeviceHistory
    {
        public int DeviceHistoryId { get; set; }
        public int DeviceId { get; set; }
        public Device Device { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public DateTime DateOfAssignment { get; set; }
        public DateTime DateOfReturn { get; set; }
    }
}
