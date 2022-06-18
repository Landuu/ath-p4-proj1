namespace ath_p4_proj1.Models
{
    internal class Employee
    {
        public int? EmployeeId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
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
            EmployeeId is not null
            || IsOnePopulatedWithoutId;
        public bool IsPopulated =>
            EmployeeId is not null
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
            EmployeeId = null;
            FirstName = null;
            LastName = null;
            PhoneNumber = null;
            Email = null;
        }
    }
}
