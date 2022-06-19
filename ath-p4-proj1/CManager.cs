using ath_p4_proj1.Enums;
using ConsoleTools;
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.InputControls;
using DustInTheWind.ConsoleTools.Controls.Tables;
using Microsoft.EntityFrameworkCore;

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
                PrintHeader($"Pracownicy / Lista pracowników ({count})");

                var table = new DataGrid();
                table.Columns.Add("L.p.");
                table.Columns.Add("ID");
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
                        employee.EmployeeId = 0;
                        item.Name = EmployeeAddNames.Id;
                        break;
                    }
                    if (!Validator.NumericId(id))
                    {
                        StringValue.QuickWrite("Błąd: ", "Niepoprawne ID, id musi składać się z cyfr dodatnich");
                        Pause.QuickDisplay();
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
                    int take = 10;
                    int count = db.Employees
                            .WhereIf(employee.EmployeeId != 0, x => x.EmployeeId == employee.EmployeeId)
                            .WhereIf(!string.IsNullOrEmpty(employee.FirstName), x => x.FirstName.Contains(employee.FirstName))
                            .WhereIf(!string.IsNullOrEmpty(employee.LastName), x => x.LastName.Contains(employee.LastName))
                            .WhereIf(!string.IsNullOrEmpty(employee.PhoneNumber), x => x.PhoneNumber.Contains(employee.PhoneNumber))
                            .WhereIf(!string.IsNullOrEmpty(employee.Email), x => x.Email.Contains(employee.Email))
                            .Count();

                    while (true)
                    {
                        Console.Clear();
                        PrintHeader($"Pracownicy / Wyszukaj pracownika / Lista pracowników {count}");

                        var table = new DataGrid();
                        table.Columns.Add("L.p.");
                        table.Columns.Add("ID");
                        table.Columns.Add("Imię");
                        table.Columns.Add("Nazwisko");
                        table.Columns.Add("Telefon");
                        table.Columns.Add("Email");


                        var employees = db.Employees
                            .WhereIf(employee.EmployeeId != 0, x => x.EmployeeId == employee.EmployeeId)
                            .WhereIf(!string.IsNullOrEmpty(employee.FirstName), x => x.FirstName.Contains(employee.FirstName))
                            .WhereIf(!string.IsNullOrEmpty(employee.LastName), x => x.LastName.Contains(employee.LastName))
                            .WhereIf(!string.IsNullOrEmpty(employee.PhoneNumber), x => x.PhoneNumber.Contains(employee.PhoneNumber))
                            .WhereIf(!string.IsNullOrEmpty(employee.Email), x => x.Email.Contains(employee.Email))
                            .Skip(skip)
                            .Take(take)
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

        public void DeviceHistoryCheck(HistoryCheckAction action, MenuItem? item = null, IReadOnlyList<MenuItem>? items = null)
        {
            using var db = new InventoryDbContext();

            switch (action)
            {
                case HistoryCheckAction.DeviceId:
                    string idd = StringValue.QuickRead(HistoryCheckNames.DeviceId);
                    if (idd == "")
                    {
                        device.DeviceId = 0;
                        item.Name = HistoryCheckNames.DeviceId + "\n";
                        break;
                    }
                    if (!Validator.NumericId(idd))
                    {
                        StringValue.QuickWrite("Błąd: ", "Niepoprawne ID, id musi składać się z cyfr dodatnich");
                        Pause.QuickDisplay();
                        break;
                    }
                    device.DeviceId = Convert.ToInt32(idd);
                    item.Name = HistoryCheckNames.DeviceId + idd + "\n";
                    break;
                case HistoryCheckAction.Clear:
                    device.Clear();
                    items[1].Name = HistoryCheckNames.DeviceId + "\n";
                    break;
                case HistoryCheckAction.Confirm:
                    var d = db.Devices
                        .Where(x => x.DeviceId == device.DeviceId)
                        .Include(x => x.History)
                        .ThenInclude(x => x.Employee)
                        .FirstOrDefault();
                    if (d is null)
                    {
                        StringValue.QuickWrite("Błąd: ", "Urządzenie o podanym ID nie istnieje w bazie danych!");
                        Pause.QuickDisplay();
                        break;
                    }

                    int skip = 0;
                    int take = 5;
                    int count = d.History.Count;

                    while (true)
                    {
                        Console.Clear();
                        PrintHeader($"Zarządzanie urządzeniami / Lista właścicieli danego urzadzenia / Lista ({count})");

                        var tableDevice = new DataGrid();
                        tableDevice.Border.Template = BorderTemplate.SingleLineBorderTemplate;
                        tableDevice.Columns.Add("ID");
                        tableDevice.Columns.Add("Producent");
                        tableDevice.Columns.Add("Model");
                        tableDevice.Columns.Add("Numer seryjny");
                        tableDevice.Columns.Add("Data wprowadzenia");
                        tableDevice.Columns.Add("Data wykluczenia");

                        var table = new DataGrid();
                        table.Border.Template = BorderTemplate.SingleLineBorderTemplate;
                        table.Columns.Add("L.p.");
                        table.Columns.Add("ID prac.");
                        table.Columns.Add("Imię i nazwisko");
                        table.Columns.Add("Data przypisania");
                        table.Columns.Add("Data zwrócenia");

                        var deol = d.DateOfEOL is null ? "null" : d.DateOfEOL.ToString();
                        tableDevice.Rows.Add(d.DeviceId, d.Manufacturer, d.Model, d.SerialNumber, d.DateOfService, deol);

                        var histories = d.History.Skip(skip).Take(take).ToList();
                        for(int i = 0; i < histories.Count; i++)
                        {
                            var h = histories[i];
                            var dr = h.DateOfReturn is null ? "null" : h.DateOfReturn.ToString();
                            table.Rows.Add(i + 1 + skip, h.Employee.EmployeeId, $"{h.Employee.FirstName} {h.Employee.LastName}", h.DateOfAssignment, dr);
                        }

                        tableDevice.Display();
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
                    break;
            }
        }

        public void DeviceHistoryReturn(HistoryReturnAction action, MenuItem? item = null, IReadOnlyList<MenuItem>? items = null)
        {
            using var db = new InventoryDbContext();

            switch (action)
            {
                case HistoryReturnAction.DeviceId:
                    string idd = StringValue.QuickRead(HistoryReturnNames.DeviceId);
                    if (idd == "")
                    {
                        device.DeviceId = 0;
                        item.Name = HistoryReturnNames.DeviceId + "\n";
                        break;
                    }
                    if (!Validator.NumericId(idd))
                    {
                        StringValue.QuickWrite("Błąd: ", "Niepoprawne ID, id musi składać się z cyfr dodatnich");
                        Pause.QuickDisplay();
                        break;
                    }
                    device.DeviceId = Convert.ToInt32(idd);
                    item.Name = HistoryReturnNames.DeviceId + idd + "\n";
                    break;
                case HistoryReturnAction.Clear:
                    device.Clear();
                    items[1].Name = HistoryAssignNames.DeviceId + "\n";
                    break;
                case HistoryReturnAction.Confirm:
                    var d = db.Devices.Where(x => x.DeviceId == device.DeviceId).FirstOrDefault();
                    if (d is null)
                    {
                        StringValue.QuickWrite("Błąd: ", "Urządzenie o podanym ID nie istnieje w bazie danych!");
                        Pause.QuickDisplay();
                        break;
                    }

                    var dIsAssigned = db.DeviceHistories
                        .Where(x => x.DeviceId == device.DeviceId)
                        .Where(x => x.DateOfReturn == null)
                        .FirstOrDefault();
                    if (dIsAssigned is null)
                    {
                        StringValue.QuickWrite("Błąd: ", "Urządzenie o podanym ID zostało już zwrócone!");
                        Pause.QuickDisplay();
                        break;
                    }

                    dIsAssigned.DateOfReturn = DateTime.Now;
                    db.SaveChanges();
                    DeviceHistoryReturn(HistoryReturnAction.Clear, items: items);
                    StringValue.QuickWrite("Ok: ", "Zwrócono urządzenie");
                    Pause.QuickDisplay();
                    break;
            }
        }

        public void DeviceHistoryAssign(HistoryAssignAction action, MenuItem? item = null, IReadOnlyList<MenuItem>? items = null)
        {
            using var db = new InventoryDbContext();

            switch (action) {
                case HistoryAssignAction.DeviceId:
                    string idd = StringValue.QuickRead(HistoryAssignNames.DeviceId);
                    if (idd == "")
                    {
                        device.DeviceId = 0;
                        item.Name = HistoryAssignNames.DeviceId;
                        break;
                    }
                    if (!Validator.NumericId(idd))
                    {
                        StringValue.QuickWrite("Błąd: ", "Niepoprawne ID, id musi składać się z cyfr dodatnich");
                        Pause.QuickDisplay();
                        break;
                    }
                    device.DeviceId = Convert.ToInt32(idd);
                    item.Name = HistoryAssignNames.DeviceId + idd;
                    break;
                case HistoryAssignAction.EmployeeId:
                    string ide = StringValue.QuickRead(HistoryAssignNames.EmployeeID);
                    if (ide == "")
                    {
                        employee.EmployeeId = 0;
                        item.Name = HistoryAssignNames.EmployeeID + "\n";
                        break;
                    }
                    if (!Validator.NumericId(ide))
                    {
                        StringValue.QuickWrite("Błąd: ", "Niepoprawne ID, id musi składać się z cyfr dodatnich");
                        Pause.QuickDisplay();
                        break;
                    }
                    employee.EmployeeId = Convert.ToInt32(ide);
                    item.Name = HistoryAssignNames.EmployeeID + ide + "\n";
                    break;
                case HistoryAssignAction.Clear:
                    employee.Clear();
                    device.Clear();
                    items[1].Name = HistoryAssignNames.DeviceId;
                    items[2].Name = HistoryAssignNames.EmployeeID + "\n";
                    break;
                case HistoryAssignAction.Confirm:
                    var d = db.Devices.Where(x => x.DeviceId == device.DeviceId).FirstOrDefault();
                    if(d is null)
                    {
                        StringValue.QuickWrite("Błąd: ", "Urządzenie o podanym ID nie istnieje w bazie danych!");
                        Pause.QuickDisplay();
                        break;
                    }

                    if(d.DateOfEOL is not null)
                    {
                        StringValue.QuickWrite("Błąd: ", "Urządzenie o podanym ID jest zarchiwizowane, nie możesz go przypisać!");
                        Pause.QuickDisplay();
                        break;
                    }

                    var dIsAssigned = db.DeviceHistories
                        .Where(x => x.DeviceId == device.DeviceId)
                        .Where(x => x.DateOfReturn == null)
                        .FirstOrDefault();
                    if(dIsAssigned is not null)
                    {
                        StringValue.QuickWrite("Błąd: ", "Urządzenie o podanym ID jest już przypisane do innego pracownika!");
                        Pause.QuickDisplay();
                        break;
                    }

                    var e = db.Employees.Where(x => x.EmployeeId == employee.EmployeeId).FirstOrDefault();
                    if (e is null)
                    {
                        StringValue.QuickWrite("Błąd: ", "Pracownik o podanym ID nie istnieje w bazie danych!");
                        Pause.QuickDisplay();
                        break;
                    }

                    var h = new Models.DeviceHistory();
                    h.DateOfAssignment = DateTime.Now;
                    h.Employee = e;
                    h.Device = d;
                    db.DeviceHistories.Add(h);
                    db.SaveChanges();
                    DeviceHistoryAssign(HistoryAssignAction.Clear, items: items);
                    StringValue.QuickWrite("Ok: ", "Przypisano urządzenie do użytkownika");
                    Pause.QuickDisplay();

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
                    if (fn == "")
                    {
                        employee.FirstName = fn;
                        item.Name = EmployeeAddNames.FirstName;
                        break;
                    }
                    if (!Validator.Name(fn))
                    {
                        StringValue.QuickWrite("Błąd:", "Niepoprawne imię");
                        Pause.QuickDisplay();
                        break;
                    }
                    employee.FirstName = fn;
                    item.Name = EmployeeAddNames.FirstName + fn;
                    break;
                case EmployeeAddAction.LastName:
                    string ln = StringValue.QuickRead(EmployeeAddNames.LastName);
                    if (ln == "")
                    {
                        employee.LastName = ln;
                        item.Name = EmployeeAddNames.LastName;
                        break;
                    }
                    if (!Validator.Name(ln))
                    {
                        StringValue.QuickWrite("Błąd:", "Niepoprawne nazwisko");
                        Pause.QuickDisplay();
                        break;
                    }
                    employee.LastName = ln;
                    item.Name = EmployeeAddNames.LastName + ln;
                    break;
                case EmployeeAddAction.Phone:
                    string p = StringValue.QuickRead(EmployeeAddNames.PhoneNumber);
                    if (p == "")
                    {
                        employee.PhoneNumber = p;
                        item.Name = EmployeeAddNames.PhoneNumber;
                        break;
                    }
                    if (!Validator.PhoneNumber(p))
                    {
                        StringValue.QuickWrite("Błąd:", "Niepoprawny numer telefonu");
                        Pause.QuickDisplay();
                        break;
                    }
                    employee.PhoneNumber = p;
                    item.Name = EmployeeAddNames.PhoneNumber + p;
                    break;
                case EmployeeAddAction.Email:
                    string e = StringValue.QuickRead(EmployeeAddNames.PhoneNumber);
                    if (e == "")
                    {
                        employee.Email = e;
                        item.Name = EmployeeAddNames.Email + "\n";
                        break;
                    }
                    if (!Validator.Email(e))
                    {
                        StringValue.QuickWrite("Błąd:", "Niepoprawny adres email");
                        Pause.QuickDisplay();
                        break;
                    }
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
                        device.DeviceId = 0;
                        item.Name = EmployeeRemoveNames.Id;
                        break;
                    }
                    if (!Validator.NumericId(id))
                    {
                        StringValue.QuickWrite("Błąd: ", "Niepoprawne ID, id musi składać się z cyfr dodatnich");
                        Pause.QuickDisplay();
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
                PrintHeader($"Urządzenia / Lista urządzeń ({count})");

                var table = new DataGrid();
                table.Columns.Add("L.p.");
                table.Columns.Add("ID");
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
                    device.DateOfService = DateTime.Now;
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
                        device.DeviceId = 0;
                        item.Name = DeviceRemoveNames.Id;
                        break;
                    }
                    if (!Validator.NumericId(id))
                    {
                        StringValue.QuickWrite("Błąd: ", "Niepoprawne ID, id musi składać się z cyfr dodatnich");
                        Pause.QuickDisplay();
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

        public void DeviceSearch(DeviceSearchAction action, MenuItem? item = null, IReadOnlyList<MenuItem>? items = null)
        {
            using var db = new InventoryDbContext();

            switch (action)
            {
                case DeviceSearchAction.Id:
                    string id = StringValue.QuickRead(DeviceSearchNames.Id);
                    if (id == "")
                    {
                        device.DeviceId = 0;
                        item.Name = DeviceSearchNames.Id;
                        break;
                    }
                    if (!Validator.NumericId(id))
                    {
                        StringValue.QuickWrite("Błąd: ", "Niepoprawne ID, id musi składać się z cyfr dodatnich");
                        Pause.QuickDisplay();
                        break;
                    }
                    device.DeviceId = Convert.ToInt32(id);
                    item.Name = DeviceSearchNames.Id + id;
                    break;
                case DeviceSearchAction.Manufacturer:
                    string mn = StringValue.QuickRead(DeviceSearchNames.Manufacturer);
                    device.Manufacturer = mn;
                    item.Name = DeviceSearchNames.Manufacturer + mn;
                    break;
                case DeviceSearchAction.Model:
                    string mo = StringValue.QuickRead(DeviceSearchNames.Model);
                    device.Model = mo;
                    item.Name = DeviceSearchNames.Model + mo;
                    break;
                case DeviceSearchAction.SerialNumber:
                    string sn = StringValue.QuickRead(DeviceSearchNames.SerialNumber);
                    device.SerialNumber = sn;
                    item.Name = DeviceSearchNames.SerialNumber + sn;
                    break;
                case DeviceSearchAction.Clear:
                    device.Clear();
                    items[1].Name = DeviceSearchNames.Id;
                    items[2].Name = DeviceSearchNames.Manufacturer;
                    items[3].Name = DeviceSearchNames.Model;
                    items[4].Name = DeviceSearchNames.SerialNumber + "\n";
                    break;
                case DeviceSearchAction.Confirm:
                    if (!device.IsOnePopulatedSearch)
                    {
                        StringValue.QuickWrite("Błąd:", "Musisz wypełnić chociaż jedno pole!");
                        Pause.QuickDisplay();
                        break;
                    }

                    int skip = 0;
                    int take = 10;
                    int count = db.Devices
                            .WhereIf(device.DeviceId != 0, x => x.DeviceId == device.DeviceId)
                            .WhereIf(!string.IsNullOrEmpty(device.Manufacturer), x => x.Manufacturer.Contains(device.Manufacturer))
                            .WhereIf(!string.IsNullOrEmpty(device.Model), x => x.Model.Contains(device.Model))
                            .WhereIf(!string.IsNullOrEmpty(device.SerialNumber), x => x.SerialNumber.Contains(device.SerialNumber))
                            .Count();

                    while (true)
                    {
                        Console.Clear();
                        PrintHeader($"Urządzenia / Wyszukaj urządzenie / Lista urządzeń ({count})");

                        var table = new DataGrid();
                        table.Columns.Add("L.p.");
                        table.Columns.Add("ID");
                        table.Columns.Add("Producent");
                        table.Columns.Add("Model");
                        table.Columns.Add("Numer seryjny");
                        table.Columns.Add("Data wpr.");
                        table.Columns.Add("Data wył.");


                        var devices = db.Devices
                            .WhereIf(device.DeviceId != 0, x => x.DeviceId == device.DeviceId)
                            .WhereIf(!string.IsNullOrEmpty(device.Manufacturer), x => x.Manufacturer.Contains(device.Manufacturer))
                            .WhereIf(!string.IsNullOrEmpty(device.Model), x => x.Model.Contains(device.Model))
                            .WhereIf(!string.IsNullOrEmpty(device.SerialNumber), x => x.SerialNumber.Contains(device.SerialNumber))
                            .Skip(skip)
                            .Take(take)
                            .ToList();

                        for (int i = 0; i < devices.Count; i++)
                        {
                            var dd = devices[i];
                            var dateOfService = dd.DateOfService is null ? "null" : dd.DateOfService.ToString();
                            var dateOfEOL = dd.DateOfEOL is null ? "null" : dd.DateOfEOL.ToString();
                            table.Rows.Add(i + 1 + skip, dd.DeviceId, dd.Manufacturer, dd.Model, dd.SerialNumber, dateOfService, dateOfEOL);
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

                    DeviceSearch(DeviceSearchAction.Clear, items: items);
                    break;
            }
        }

        public void DeviceArchive(DeviceRemoveAction action, MenuItem? item = null, IReadOnlyList<MenuItem>? items = null)
        {
            using var db = new InventoryDbContext();

            switch (action)
            {
                case DeviceRemoveAction.Id:
                    string id = StringValue.QuickRead(EmployeeAddNames.Id);
                    if (id == "")
                    {
                        device.DeviceId = 0;
                        item.Name = DeviceRemoveNames.Id;
                        break;
                    }
                    if(!Validator.NumericId(id))
                    {
                        StringValue.QuickWrite("Błąd: ", "Niepoprawne ID, id musi składać się z cyfr dodatnich");
                        Pause.QuickDisplay();
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
                    if (!device.IsOnePopulated)
                    {
                        StringValue.QuickWrite("Błąd!", "Musisz wypełnic wszystkie pola!");
                        Pause.QuickDisplay();
                        break;
                    }

                    var d = db.Devices
                        .Where(x => x.DeviceId == device.DeviceId)
                        .Include(x => x.History)
                        .FirstOrDefault();

                    if(d is null)
                    {
                        StringValue.QuickWrite("Błąd:", "Urządzenie o podanym ID nie istnieje w bazie danych!");
                        Pause.QuickDisplay();
                        break;
                    }

                    var dIsAssigned = d.History.Where(x => x.DateOfReturn is null).FirstOrDefault();
                    if(dIsAssigned is not null)
                    {
                        StringValue.QuickWrite("Błąd:", "Urządzenie o podanym ID jest w posiadaniu pracownika, musisz najpierw zwrócić urządzneie!");
                        Pause.QuickDisplay();
                        break;
                    }

                    d.DateOfEOL = DateTime.Now;
                    db.SaveChanges();
                    DeviceArchive(DeviceRemoveAction.Clear, items: items);
                    StringValue.QuickWrite("Ok:", "Zarchiwizowano urządzenie");
                    Pause.QuickDisplay();
                    break;
            }
        }
    }
}
