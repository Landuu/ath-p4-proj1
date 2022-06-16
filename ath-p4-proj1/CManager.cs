using ConsoleTools;
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.InputControls;
using DustInTheWind.ConsoleTools.Controls.Tables;


namespace ath_p4_proj1
{
    internal class CManager
    {
        private InventoryDbContext db;
        private Models.Employee employee;

        public CManager(InventoryDbContext context)
        {
            db = context;
            employee = new Models.Employee();
        }

        private void PrintHeader(string name)
        {
            var title = new TextBlock
            {
                Text = "Pracownicy / Lista pracowników",
                Margin = new Thickness(0, 0, 0, 1)
            };
            title.Display();
            var controls = new TextBlock
            {
                Text = "Any - Powrót, PageUp - Kolejna strona, PageDown - Poprzednia strona",
                Margin = new Thickness(0, 0, 0, 1),
                ForegroundColor = ConsoleColor.DarkGray
            };
            controls.Display();
        }


        public void EmployeeList()
        {
            int skip = 0;
            int take = 10;
            int count = db.Employees.Count();

            while (true)
            {
                Console.Clear();
                PrintHeader("Lista pracowników");

                var table = new DataGrid();
                table.Columns.Add("L.p.");
                table.Columns.Add("ID pracownika");
                table.Columns.Add("Imię");
                table.Columns.Add("Nazwisko");
                table.Columns.Add("Telefon");
                table.Columns.Add("Email");


                var employees = db.Employees.Skip(skip).Take(take).ToList();
                for (int i = 0; i < employees.Count; i++)
                {
                    var e = employees[i];
                    table.Rows.Add(i + 1 + skip, e.EmployeeId, e.FirstName, e.LastName, e.PhoneNumber, e.Email);
                }

                table.Border.Template = BorderTemplate.SingleLineBorderTemplate;
                table.Display();

                var key = Console.ReadKey().Key;
                if (key == ConsoleKey.PageUp)
                {
                    if (skip + take > count) continue;
                    skip += take;
                }
                else if (key == ConsoleKey.PageDown)
                {
                    if (skip - take < 0) continue;
                    skip -= take;
                }
                else
                {
                    break;
                }
            }
        }

        public void EmployeeAdd(EmployeeAddAction action, MenuItem? item = null, IReadOnlyList<MenuItem>? items = null)
        {
            switch(action)
            {
                case EmployeeAddAction.FirstName:
                    string fn = StringValue.QuickRead(EmployeeAddNames.FirstName);
                    employee.FirstName = fn;
                    item.Name = EmployeeAddNames.FirstName + fn;
                    break;
                case EmployeeAddAction.LastName:
                    string ln = StringValue.QuickRead(EmployeeAddNames.LastName);
                    employee.LastName = ln;
                    item.Name = EmployeeAddNames.LastName + ln;
                    break;
                case EmployeeAddAction.Phone:
                    string p = StringValue.QuickRead(EmployeeAddNames.PhoneNumber);
                    employee.PhoneNumber = p;
                    item.Name = EmployeeAddNames.PhoneNumber + p;
                    break;
                case EmployeeAddAction.Email:
                    string e = StringValue.QuickRead(EmployeeAddNames.PhoneNumber);
                    employee.Email = e;
                    item.Name = EmployeeAddNames.Email + e + "\n";
                    break;
                case EmployeeAddAction.Clear:
                    employee.Clear();
                    items[1].Name = EmployeeAddNames.FirstName;
                    items[2].Name = EmployeeAddNames.LastName;
                    items[3].Name = EmployeeAddNames.PhoneNumber;
                    items[4].Name = EmployeeAddNames.Email + "\n";
                    break;
                case EmployeeAddAction.Confirm:
                    if(!employee.IsPopulated())
                    {
                        StringValue.QuickWrite("Błąd:", "Musisz wypełnić wszystkie pola!");
                        Pause.QuickDisplay();
                        break;
                    }

                    db.Employees.Add(employee);
                    db.SaveChanges();
                    StringValue.QuickWrite("Ok:", "Dodano użytkownika do bazy");
                    Pause.QuickDisplay();

                    break;

            }
        }
    }
}
