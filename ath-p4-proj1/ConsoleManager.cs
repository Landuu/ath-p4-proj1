
namespace ath_p4_proj1
{
    internal class ConsoleManager
    {
        private ConsoleMenus menus;
        private Position previousPosition;
        private Position position;

        public ConsoleManager(ConsoleMenus menus)
        {
            previousPosition = new Position();
            position = Position.Main;
            this.menus = menus;
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
                if (choice == "1") EmoployeeAdd();
                else if (choice == "2") newPosition = Position.EmployeesEdit;
                else if (choice == "3") newPosition = Position.EmployeesDelete;
                else if (choice == "4") newPosition = previousPosition;
                else exit = true;
            } else if(position == Position.Devices)
            {
                if (choice == "4") newPosition = previousPosition;
                else exit = true;
            } else if(position == Position.History)
            {
                if (choice == "3") newPosition = previousPosition;
                else exit = true;
            } else if(position == position)
            

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

            menus.Draw(pos);
            ReadChoice();
        }

        private void EmoployeeAdd()
        {
            var headerName = "Dodawanie pracownika";
            var headerBorder = new String('-', headerName.Length);

            Console.Clear();
            Console.WriteLine(headerBorder);
            Console.WriteLine(headerName);
            Console.WriteLine(headerBorder);

            Console.Write("ID:");
            Console.ReadLine();

            MoveTo(Position.Employees);
        }
    }
}
