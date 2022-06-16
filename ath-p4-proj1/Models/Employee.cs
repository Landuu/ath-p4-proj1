namespace ath_p4_proj1.Models
{
    internal class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public Employee() { }

        public Employee(string firstName, string lastName, string phoneNumber, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        public bool IsPopulated()
        {
            return !(
                String.IsNullOrEmpty(FirstName)
                && String.IsNullOrEmpty(LastName)
                && String.IsNullOrEmpty(PhoneNumber)
                && String.IsNullOrEmpty(Email)
            );
        }

        public void Clear()
        {
            FirstName = "";
            LastName = "";
            PhoneNumber = "";
            Email = "";
        }
    }
}
