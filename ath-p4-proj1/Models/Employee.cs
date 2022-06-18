namespace ath_p4_proj1.Models
{
    internal class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

        public bool IsOnePopulatedWithoutId =>
            !string.IsNullOrEmpty(FirstName)
            || !string.IsNullOrEmpty(LastName)
            || !string.IsNullOrEmpty(PhoneNumber)
            || !string.IsNullOrEmpty(Email);

        public bool IsPopulatedWithoutId => 
            !string.IsNullOrEmpty(FirstName)
            && !string.IsNullOrEmpty(LastName)
            && !string.IsNullOrEmpty(PhoneNumber)
            && !string.IsNullOrEmpty(Email);
        public bool IsOnePopulated =>
            EmployeeId != 0
            || IsOnePopulatedWithoutId;
        public bool IsPopulated =>
            EmployeeId != 0
            && IsPopulatedWithoutId;


        public Employee() { }

        public Employee(string firstName, string lastName, string phoneNumber, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        public void Clear()
        {
            EmployeeId = 0;
            FirstName = "";
            LastName = "";
            PhoneNumber = null;
            Email = null;
        }
    }
}
