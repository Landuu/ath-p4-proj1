
using System.Text.RegularExpressions;

namespace ath_p4_proj1
{
    internal class ConsoleManager
    {
        private InventoryDbContext db;
        private ConsoleMenus menus;
        private Position previousPosition;
        private Position position;

        public ConsoleManager(ConsoleMenus menus, InventoryDbContext db)
        {
            previousPosition = new Position();
            position = Position.Main;
            this.menus = menus;
            this.db = db;
        }

        public void Start()
        {
            Console.Clear();
            menus.Draw(Position.Main);
            ReadChoice();
        }

        private void ReadChoice()
        {
            bool exit = false;
            var choice = Console.ReadLine();
            var newPosition = new Position();

            if(position == Position.Main)
            {
                if (choice == "1") newPosition = Position.Employees;
                else if (choice == "2") newPosition = Position.Devices;
                else if (choice == "3") newPosition = Position.History;
                else exit = true;
            } else if(position == Position.Employees)
            {
                if (choice == "1")
                {
                    EmployeeList();
                    newPosition = Position.Employees;
                }
                else if (choice == "2")
                {
                    EmployeeAdd();
                    newPosition = Position.Employees;
                }
                else if (choice == "3") newPosition = Position.EmployeesEdit;
                else if (choice == "4") newPosition = Position.EmployeesDelete;
                else if (choice == "0") newPosition = previousPosition;
                else exit = true;
            } else if(position == Position.Devices)
            {
                if (choice == "4") newPosition = previousPosition;
                else exit = true;
            } else if(position == Position.History)
            {
                if (choice == "3") newPosition = previousPosition;
                else exit = true;
            } else
            {
                exit = true;
            }
            

            if (exit) return;

            MoveTo(newPosition);
        }

        private void MoveTo(Position pos)
        {
            previousPosition = position;
            position = pos;
            if (position == Position.Main)
            {
                Start();
                return;
            }

            if(position == previousPosition)
            {
                //Quickfix
                previousPosition = Position.Main;
            }

            menus.Draw(pos);
            ReadChoice();
        }

        private void PrintError(string msg)
        {
            Console.WriteLine("- Nieprawidłowa wartość! Wymagania: " + msg);
        }

        private void PrintHeader(string name)
        {
            var border = new String('-', name.Length);
            Console.WriteLine(border);
            Console.WriteLine(name);
            Console.WriteLine(border);
            Console.WriteLine("");
        }

        private void EmployeeList()
        {
            int skip = 0;
            int take = 10;
            int count = db.Employees.Count();

            while(true)
            {
                Console.Clear();
                PrintHeader("Lista pracowników");

                var employees = db.Employees.Skip(skip).Take(take).ToList();
                for (int i = 1; i <= employees.Count; i++)
                {
                    var e = employees[i - 1];
                    Console.WriteLine($"{i}) Id: {e.EmployeeId}, Name: {e.FirstName} {e.LastName}, Tel: {e.PhoneNumber}, Mail: {e.Email}");
                }

                Console.WriteLine("\n\n  --- 0 - Powrót, PageUp - Kolejna strona, PageDown - Poprzednia strona ---");
                var key = Console.ReadKey().Key;

                if(key == ConsoleKey.PageUp)
                {
                    if (skip + take > count) continue;
                    skip += take;
                } else if(key == ConsoleKey.PageDown)
                {
                    if (skip - take < 0) continue;
                    skip -= take;
                } else
                {
                    break;
                }
            }
        }

        private void EmployeeAdd()
        {
            var headerName = "Dodawanie pracownika";
            var headerBorder = new String('-', headerName.Length);

            Console.Clear();
            Console.WriteLine(headerBorder);
            Console.WriteLine(headerName);
            Console.WriteLine(headerBorder);

            string firstName;
            do
            {
                Console.Write("Imię:");
                firstName = Console.ReadLine();
                if (!Validator.Name(firstName)) PrintError("string, 2 - 50 znaków");
            } while(!Validator.Name(firstName));

            string lastName;
            do
            {
                Console.Write("Nazwisko:");
                lastName = Console.ReadLine();
                if (!Validator.Name(lastName)) PrintError("string, 2 - 50 znaków");
            } while (!Validator.Name(lastName));

            string phoneNumber;
            do
            {
                Console.Write("Telefon kontaktowy: ");
                phoneNumber = Console.ReadLine();
                if(!Validator.PhoneNumber(phoneNumber)) PrintError("int, 9 znaków");
            } while (!Validator.PhoneNumber(phoneNumber));


            string email;
            do
            {
                Console.Write("Adres email:");
                email = Console.ReadLine();
                if (!Validator.Email(email)) PrintError("string, musi zawierać @ oraz domenę");
            } while (!Validator.Email(email));
            

            var employee = new Models.Employee(firstName, lastName, phoneNumber, email);
            db.Employees.Add(employee);
            db.SaveChanges();
            MoveTo(Position.Employees);
        }
    }
}
