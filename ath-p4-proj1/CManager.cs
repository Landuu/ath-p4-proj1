using ath_p4_proj1.Enums;
using ConsoleTools;
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.InputControls;
using DustInTheWind.ConsoleTools.Controls.Tables;


namespace ath_p4_proj1
{
    internal class CManager
    {
        private Models.Employee employee;
        private Models.Device device;

        public CManager()
        {
            employee = new Models.Employee();
            device = new Models.Device();
        }

        private void PrintHeader(string name)
        {
            var title = new TextBlock
            {
                Text = name,
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
            using var db = new InventoryDbContext();

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

        public void EmployeeSearch(EmployeeSearchAction action, MenuItem? item = null, IReadOnlyList<MenuItem>? items = null)
        {
            using var db = new InventoryDbContext();

            switch (action)
            {
                case EmployeeSearchAction.Id:
                    string id = StringValue.QuickRead(EmployeeAddNames.Id);
                    if (id == "")
                    {
                        employee.EmployeeId = null;
                        item.Name = EmployeeAddNames.Id;
                        break;
                    }
                    employee.EmployeeId = Convert.ToInt32(id);
                    item.Name = EmployeeAddNames.Id + id;
                    break;
                case EmployeeSearchAction.FirstName:
                    string fn = StringValue.QuickRead(EmployeeAddNames.FirstName);
                    employee.FirstName = fn;
                    item.Name = EmployeeAddNames.FirstName + fn;
                    break;
                case EmployeeSearchAction.LastName:
                    string ln = StringValue.QuickRead(EmployeeAddNames.LastName);
                    employee.LastName = ln;
                    item.Name = EmployeeAddNames.LastName + ln;
                    break;
                case EmployeeSearchAction.Phone:
                    string p = StringValue.QuickRead(EmployeeAddNames.PhoneNumber);
                    employee.PhoneNumber = p;
                    item.Name = EmployeeAddNames.PhoneNumber + p;
                    break;
                case EmployeeSearchAction.Email:
                    string e = StringValue.QuickRead(EmployeeAddNames.PhoneNumber);
                    employee.Email = e;
                    item.Name = EmployeeAddNames.Email + e + "\n";
                    break;
                case EmployeeSearchAction.Clear:
                    employee.Clear();
                    items[1].Name = EmployeeAddNames.Id;
                    items[2].Name = EmployeeAddNames.FirstName;
                    items[3].Name = EmployeeAddNames.LastName;
                    items[4].Name = EmployeeAddNames.PhoneNumber;
                    items[5].Name = EmployeeAddNames.Email + "\n";
                    break;
                case EmployeeSearchAction.Confirm:
                    if(!employee.IsOnePopulated)
                    {
                        StringValue.QuickWrite("Błąd:", "Musisz wypełnić chociaż jedno pole!");
                        Pause.QuickDisplay();
                        break;
                    }

                    int skip = 0;
                    int take = 5;
                    int count = 0;

                    while (true)
                    {
                        Console.Clear();
                        PrintHeader("Pracownicy / Wyszukaj pracownika / Lista pracowników");

                        var table = new DataGrid();
                        table.Columns.Add("L.p.");
                        table.Columns.Add("ID pracownika");
                        table.Columns.Add("Imię");
                        table.Columns.Add("Nazwisko");
                        table.Columns.Add("Telefon");
                        table.Columns.Add("Email");


                        var employees = db.Employees
                            .WhereIf(employee.EmployeeId is not null, x => x.EmployeeId == employee.EmployeeId)
                            .WhereIf(!string.IsNullOrEmpty(employee.FirstName), x => x.FirstName.Contains(employee.FirstName))
                            .WhereIf(!string.IsNullOrEmpty(employee.LastName), x => x.LastName.Contains(employee.LastName))
                            .WhereIf(!string.IsNullOrEmpty(employee.PhoneNumber), x => x.PhoneNumber.Contains(employee.PhoneNumber))
                            .WhereIf(!string.IsNullOrEmpty(employee.Email), x => x.Email.Contains(employee.Email))
                            .ToList();
                        for (int i = 0; i < employees.Count; i++)
                        {
                            var ee = employees[i];
                            table.Rows.Add(i + 1 + skip, ee.EmployeeId, ee.FirstName, ee.LastName, ee.PhoneNumber, ee.Email);
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

                    EmployeeSearch(EmployeeSearchAction.Clear, items: items);
                    break;

            }
        }

        public void EmployeeAdd(EmployeeAddAction action, MenuItem? item = null, IReadOnlyList<MenuItem>? items = null)
        {
            using var db = new InventoryDbContext();

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
                    if(!employee.IsPopulatedWithoutId)
                    {
                        StringValue.QuickWrite("Błąd:", "Musisz wypełnić wszystkie pola!");
                        Pause.QuickDisplay();
                        break;
                    }

                    db.Employees.Add(employee);
                    db.SaveChanges();
                    EmployeeAdd(EmployeeAddAction.Clear, items: items);
                    StringValue.QuickWrite("Ok:", "Dodano użytkownika do bazy");
                    Pause.QuickDisplay();
                    break;

            }
        }

        public void EmployeeRemove(EmployeeRemoveAction action, MenuItem? item = null, IReadOnlyList<MenuItem>? items = null)
        {
            using var db = new InventoryDbContext();

            switch (action)
            {
                case EmployeeRemoveAction.Id:
                    string id = StringValue.QuickRead(EmployeeAddNames.Id);
                    if (id == "")
                    {
                        device.DeviceId = null;
                        item.Name = EmployeeRemoveNames.Id;
                        break;
                    }
                    employee.EmployeeId = Convert.ToInt32(id);
                    item.Name = EmployeeRemoveNames.Id + id + "\n";
                    break;
                case EmployeeRemoveAction.Clear:
                    employee.Clear();
                    items[1].Name = EmployeeRemoveNames.Id + "\n";
                    break;
                case EmployeeRemoveAction.Confirm:
                    if (!employee.IsOnePopulated)
                    {
                        StringValue.QuickWrite("Błąd!", "Musisz wypełnic wszystkie pola!");
                        Pause.QuickDisplay();
                        break;
                    }

                    var e = db.Employees.Where(x => x.EmployeeId == employee.EmployeeId).FirstOrDefault();
                    StringValue.QuickWrite("Usuwany rekord: \n", $"{e.EmployeeId}, {e.FirstName} {e.LastName}, {e.PhoneNumber}, {e.Email}");
                    var q = new YesNoQuestion("Czy na pewno chcesz usunąć powyższy rekord?");
                    var a = q.ReadAnswer();
                    if (a == YesNoAnswer.No)
                    {
                        StringValue.QuickWrite("Błąd!", "Anulowano");
                        break;
                    }

                    db.Employees.Remove(e);
                    db.SaveChanges();
                    EmployeeRemove(EmployeeRemoveAction.Clear, items: items);
                    StringValue.QuickWrite("Ok:", "Usunięto rekord z bazy danych");
                    Pause.QuickDisplay();
                    break;
            }
        }

        public void DevicesList()
        {
            using var db = new InventoryDbContext();

            int skip = 0;
            int take = 10;
            int count = db.Devices.Count();

            while (true)
            {
                Console.Clear();
                PrintHeader("Urządzenia / Lista urządzeń");

                var table = new DataGrid();
                table.Columns.Add("L.p.");
                table.Columns.Add("ID urządzenia");
                table.Columns.Add("Producent");
                table.Columns.Add("Model");
                table.Columns.Add("Numer seryjny");
                table.Columns.Add("Data wprowadzenia do uż.");
                table.Columns.Add("Data wyłączenia z uż.");


                var devices = db.Devices.Skip(skip).Take(take).ToList();
                for (int i = 0; i < devices.Count; i++)
                {
                    var d = devices[i];
                    var dateEOL = d.DateOfEOL is null ? "null" : d.DateOfEOL.ToString();
                    table.Rows.Add(i + 1 + skip, d.DeviceId, d.Manufacturer, d.Model, d.SerialNumber, d.DateOfService, dateEOL);
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

        public void DeviceAdd(DeviceAddAction action, MenuItem? item = null, IReadOnlyList<MenuItem>? items = null)
        {
            using var db = new InventoryDbContext();

            switch (action)
            {
                case DeviceAddAction.Manufacturer:
                    string ma = StringValue.QuickRead(DeviceAddNames.Manufacturer);
                    device.Manufacturer = ma;
                    item.Name = DeviceAddNames.Manufacturer + ma;
                    break;
                case DeviceAddAction.Model:
                    string mo = StringValue.QuickRead(DeviceAddNames.Model);
                    device.Model = mo;
                    item.Name = DeviceAddNames.Model + mo;
                    break;
                case DeviceAddAction.SerialNumber:
                    string sn = StringValue.QuickRead(DeviceAddNames.SerialNumber);
                    device.SerialNumber = sn;
                    item.Name = DeviceAddNames.SerialNumber + sn + "\n";
                    break;
                case DeviceAddAction.Clear:
                    device.Clear();
                    items[1].Name = DeviceAddNames.Manufacturer;
                    items[2].Name = DeviceAddNames.Model;
                    items[3].Name = DeviceAddNames.SerialNumber + "\n";
                    break;
                case DeviceAddAction.Confirm:
                    device.DateOfService = DateTime.UtcNow;
                    if (!device.IsPopulatedWithoutId)
                    {
                        StringValue.QuickWrite("Błąd!", "Musisz wypełnic wszystkie pola!");
                        Pause.QuickDisplay();
                        break;
                    }
                    

                    db.Devices.Add(device);
                    db.SaveChanges();
                    DeviceAdd(DeviceAddAction.Clear, items: items);
                    StringValue.QuickWrite("Ok:", "Dodano urządzenie do bazy");
                    Pause.QuickDisplay();
                    break;
            }

        }

        public void DeviceRemove(DeviceRemoveAction action, MenuItem? item = null, IReadOnlyList<MenuItem>? items = null)
        {
            using var db = new InventoryDbContext();

            switch(action)
            {
                case DeviceRemoveAction.Id:
                    string id = StringValue.QuickRead(EmployeeAddNames.Id);
                    if (id == "")
                    {
                        device.DeviceId = null;
                        item.Name = DeviceRemoveNames.Id;
                        break;
                    }
                    device.DeviceId = Convert.ToInt32(id);
                    item.Name = DeviceRemoveNames.Id + id + "\n";
                    break;
                case DeviceRemoveAction.Clear:
                    device.Clear();
                    items[1].Name = DeviceRemoveNames.Id + "\n";
                    break;
                case DeviceRemoveAction.Confirm:
                    if(!device.IsOnePopulated)
                    {
                        StringValue.QuickWrite("Błąd!", "Musisz wypełnic wszystkie pola!");
                        Pause.QuickDisplay();
                        break;
                    }

                    var d = db.Devices.Where(x => x.DeviceId == device.DeviceId).FirstOrDefault();
                    StringValue.QuickWrite("Usuwany rekord: \n", $"{d.DeviceId}, {d.Manufacturer} {d.Model}, {d.SerialNumber}");
                    var q = new YesNoQuestion("Czy na pewno chcesz usunąć powyższy rekord?");
                    var a = q.ReadAnswer();
                    if(a == YesNoAnswer.No)
                    {
                        StringValue.QuickWrite("Błąd!", "Anulowano");
                        break;
                    }

                    db.Devices.Remove(d);
                    db.SaveChanges();
                    DeviceRemove(DeviceRemoveAction.Clear, items: items);
                    StringValue.QuickWrite("Ok:", "Usunięto rekord z bazy danych");
                    Pause.QuickDisplay();
                    break;
            }
        }
    }
}
