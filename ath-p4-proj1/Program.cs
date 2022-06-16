using ath_p4_proj1;
using ConsoleTools;
using DustInTheWind.ConsoleTools.Controls;

using var context = new InventoryDbContext();
context.Database.EnsureCreated();

/*var menus = new ConsoleMenus();
var manager = new ConsoleManager(menus, context);*/

var manager = new CManager(context);
var menuSelector = "-->";
var menuHeaderAction = () => Console.WriteLine("Wybór opcji:");


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
    .Add("Dodaj", () => manager.EmployeeAdd(EmployeeAddAction.Confirm))
    .Configure(config =>
    {
        config.Title = "Pracownicy / Dodaj pracownika \n";
        config.EnableWriteTitle = true;
        config.WriteHeaderAction = menuHeaderAction;
        config.Selector = menuSelector;
    });

var menuEmployees = new ConsoleMenu(args, level: 1)
    .Add("Lista pracowników", () => manager.EmployeeList())
    .Add("Dodaj pracownika", () => menuEmployeesAdd.Show())
    .Add("Edytuj pracowika", () => { })
    .Add("Usuń pracownika \n", () => { })
    .Add("Powrót", ConsoleMenu.Close)
    .Configure(config =>
    {
        config.Title = "Pracownicy \n";
        config.EnableWriteTitle = true;
        config.WriteHeaderAction = menuHeaderAction;
        config.Selector = menuSelector;
    });


// Urządzenia

var menuDevices = new ConsoleMenu(args, level: 1)
    .Add("Lista urządzeń", () => { })
    .Add("Dodaj urządzenie", () => { })
    .Add("Edytuj urządzenie", () => { })
    .Add("Usuń urządzenie \n", () => { })
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