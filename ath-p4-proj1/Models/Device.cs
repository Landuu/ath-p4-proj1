
namespace ath_p4_proj1.Models
{
    internal class Device
    {
        public int DeviceId { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public DateTime DateOfService { get; set; }
        public DateTime DateOfEOL { get; set; }
    }
}
