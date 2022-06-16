namespace ath_p4_proj1.Models
{
    internal class Employee
    {
        public int EmployeeId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public Employee(string phoneNumber, string email)
        {
            PhoneNumber = phoneNumber;
            Email = email;
        }
    }
}
