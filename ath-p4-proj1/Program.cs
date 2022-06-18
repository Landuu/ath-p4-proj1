using ath_p4_proj1;
using ath_p4_proj1.Enums;
using ConsoleTools;
using DustInTheWind.ConsoleTools.Controls;

using var context = new InventoryDbContext();
context.Database.EnsureCreated();

var manager = new CManager();
var menuSelector = "-->";
var menuHeaderAction = () => Console.WriteLine(@"\\ Wybór opcji:");



// Pracownicy

var menuEmployeesAdd = new ConsoleMenu(args, level: 2)
    .Add("Powrót \n", (thisMenu) =>
    {
        manager.EmployeeAdd(EmployeeAddAction.Clear, items: thisMenu.Items);
        thisMenu.CloseMenu();
    })
    .Add(EmployeeAddNames.FirstName, (thisMenu) => manager.EmployeeAdd(EmployeeAddAction.FirstName, thisMenu.CurrentItem))
    .Add(EmployeeAddNames.LastName, (thisMenu) => manager.EmployeeAdd(EmployeeAddAction.LastName, thisMenu.CurrentItem))
    .Add(EmployeeAddNames.PhoneNumber, (thisMenu) => manager.EmployeeAdd(EmployeeAddAction.Phone, thisMenu.CurrentItem))
    .Add(EmployeeAddNames.Email + "\n", (thisMenu) => manager.EmployeeAdd(EmployeeAddAction.Email, thisMenu.CurrentItem))
    .Add("Wyczyść pola", (thisMenu) => manager.EmployeeAdd(EmployeeAddAction.Clear, items: thisMenu.Items))
    .Add("Dodaj", (thisMenu) => manager.EmployeeAdd(EmployeeAddAction.Confirm, items: thisMenu.Items))
    .Configure(config =>
    {
        config.Title = "Pracownicy / Dodaj pracownika \n";
        config.EnableWriteTitle = true;
        config.WriteHeaderAction = menuHeaderAction;
        config.Selector = menuSelector;
    });

var menuEmployeesSearch = new ConsoleMenu(args, level: 2)
    .Add("Powrót \n", (thisMenu) =>
    {
        manager.EmployeeSearch(EmployeeSearchAction.Clear, items: thisMenu.Items);
        thisMenu.CloseMenu();
    })
    .Add(EmployeeAddNames.Id, (thisMenu) => manager.EmployeeSearch(EmployeeSearchAction.Id, thisMenu.CurrentItem))
    .Add(EmployeeAddNames.FirstName, (thisMenu) => manager.EmployeeSearch(EmployeeSearchAction.FirstName, thisMenu.CurrentItem))
    .Add(EmployeeAddNames.LastName, (thisMenu) => manager.EmployeeSearch(EmployeeSearchAction.LastName, thisMenu.CurrentItem))
    .Add(EmployeeAddNames.PhoneNumber, (thisMenu) => manager.EmployeeSearch(EmployeeSearchAction.Phone, thisMenu.CurrentItem))
    .Add(EmployeeAddNames.Email + "\n", (thisMenu) => manager.EmployeeSearch(EmployeeSearchAction.Email, thisMenu.CurrentItem))
    
    .Add("Wyczyść pola", (thisMenu) => manager.EmployeeSearch(EmployeeSearchAction.Clear, items: thisMenu.Items))
    .Add("Wyszukaj", (thisMenu) => manager.EmployeeSearch(EmployeeSearchAction.Confirm, items: thisMenu.Items))
    .Configure(config =>
    {
        config.Title = "Pracownicy / Wyszukaj pracownika \n";
        config.EnableWriteTitle = true;
        config.WriteHeaderAction = menuHeaderAction;
        config.Selector = menuSelector;
    });

var menuEmployeesRemove = new ConsoleMenu(args, level: 2)
    .Add("Powrót \n", (thisMenu) =>
    {
        manager.EmployeeRemove(EmployeeRemoveAction.Clear, items: thisMenu.Items);
        thisMenu.CloseMenu();
    })
    .Add(EmployeeRemoveNames.Id + "\n", (thisMenu) => manager.EmployeeRemove(EmployeeRemoveAction.Id, thisMenu.CurrentItem))
    .Add("Wyczyść pola", (thisMenu) => manager.EmployeeRemove(EmployeeRemoveAction.Clear, items: thisMenu.Items))
    .Add("Dodaj", (thisMenu) => manager.EmployeeRemove(EmployeeRemoveAction.Confirm, items: thisMenu.Items))
    .Configure(config =>
    {
        config.Title = "Pracownicy / Usuń pracownika \n";
        config.EnableWriteTitle = true;
        config.WriteHeaderAction = menuHeaderAction;
        config.Selector = menuSelector;
    });

var menuEmployees = new ConsoleMenu(args, level: 1)
    .Add("Lista pracowników", () => manager.EmployeeList())
    .Add("Wyszukaj pracownika", () => menuEmployeesSearch.Show())
    .Add("Dodaj pracownika", () => menuEmployeesAdd.Show())
    .Add("Edytuj pracowika", () => { })
    .Add("Usuń pracownika \n", () => menuEmployeesRemove.Show())
    .Add("Powrót", ConsoleMenu.Close)
    .Configure(config =>
    {
        config.Title = "Pracownicy \n";
        config.EnableWriteTitle = true;
        config.WriteHeaderAction = menuHeaderAction;
        config.Selector = menuSelector;
    });



// Urządzenia

var menuDevicesRemove = new ConsoleMenu(args, level: 2)
    .Add("Powrót \n", (thisMenu) =>
    {
        manager.DeviceRemove(DeviceRemoveAction.Clear, items: thisMenu.Items);
        thisMenu.CloseMenu();
    })
    .Add(DeviceRemoveNames.Id + "\n", (thisMenu) => manager.DeviceRemove(DeviceRemoveAction.Id, thisMenu.CurrentItem))
    .Add("Wyczyść pola", (thisMenu) => manager.DeviceRemove(DeviceRemoveAction.Clear, items: thisMenu.Items))
    .Add("Dodaj", (thisMenu) => manager.DeviceRemove(DeviceRemoveAction.Confirm, items: thisMenu.Items))
    .Configure(config =>
    {
        config.Title = "Urządzenia / Usuń urządzenie \n";
        config.EnableWriteTitle = true;
        config.WriteHeaderAction = menuHeaderAction;
        config.Selector = menuSelector;
    });

var menuDevicesAdd = new ConsoleMenu(args, level: 2)
    .Add("Powrót \n", (thisMenu) =>
    {
        manager.DeviceAdd(DeviceAddAction.Clear, items: thisMenu.Items);
        thisMenu.CloseMenu();
    })
    .Add(DeviceAddNames.Manufacturer, (thisMenu) => manager.DeviceAdd(DeviceAddAction.Manufacturer, thisMenu.CurrentItem))
    .Add(DeviceAddNames.Model, (thisMenu) => manager.DeviceAdd(DeviceAddAction.Model, thisMenu.CurrentItem))
    .Add(DeviceAddNames.SerialNumber + "\n", (thisMenu) => manager.DeviceAdd(DeviceAddAction.SerialNumber, thisMenu.CurrentItem))
    .Add("Wyczyść pola", (thisMenu) => manager.DeviceAdd(DeviceAddAction.Clear, items: thisMenu.Items))
    .Add("Dodaj", (thisMenu) => manager.DeviceAdd(DeviceAddAction.Confirm, items: thisMenu.Items))
    .Configure(config =>
    {
        config.Title = "Urządzenia / Dodaj urządzenie \n";
        config.EnableWriteTitle = true;
        config.WriteHeaderAction = menuHeaderAction;
        config.Selector = menuSelector;
    });

var menuDevices = new ConsoleMenu(args, level: 1)
    .Add("Lista urządzeń", () => manager.DevicesList())
    .Add("Dodaj urządzenie", () => menuDevicesAdd.Show())
    .Add("Edytuj urządzenie", () => { })
    .Add("Usuń urządzenie \n", () => menuDevicesRemove.Show())
    .Add("Powrót", ConsoleMenu.Close)
    .Configure(config =>
    {
        config.Title = "Urządzenia \n";
        config.EnableWriteTitle = true;
        config.WriteHeaderAction = menuHeaderAction;
        config.Selector = menuSelector;
    });



// Historia urządzeń

var menuHistory = new ConsoleMenu(args, level: 1)
    .Add("Lista Historia", () => { })
    .Add("Dodaj Historia", () => { })
    .Add("Edytuj Historia", () => { })
    .Add("Usuń Historia \n", () => { })
    .Add("Powrót", ConsoleMenu.Close)
    .Configure(config =>
    {
        config.Title = "Historia \n";
        config.EnableWriteTitle = true;
        config.WriteHeaderAction = menuHeaderAction;
        config.Selector = menuSelector;
    });


// Menu główne

var menuMain = new ConsoleMenu(args, level: 0)
    .Add("Pracownicy", () => menuEmployees.Show())
    .Add("Urządzenia", () => menuDevices.Show())
    .Add("Historia urządzeń \n", () => menuHistory.Show())
    .Add("Exit", () => Environment.Exit(0))
    .Configure(config =>
    {
        config.Title = "Menu główne \n";
        config.EnableWriteTitle = true;
        config.WriteHeaderAction = menuHeaderAction;
        config.Selector = menuSelector;
    });

menuMain.Show();
