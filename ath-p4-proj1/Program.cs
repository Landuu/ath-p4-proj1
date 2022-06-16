using ath_p4_proj1;
using Microsoft.EntityFrameworkCore;

/*var context = new InventoryDbContext();
context.Database.EnsureCreated();*/

var menus = new ConsoleMenus();
var manager = new ConsoleManager(menus);
manager.Start();