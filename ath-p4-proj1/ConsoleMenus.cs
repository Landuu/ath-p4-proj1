
namespace ath_p4_proj1
{
    internal class ConsoleMenus
    {

        private readonly ConsolePath[] options = new ConsolePath[0];

        public ConsoleMenus()
        {
            var count = Enum.GetNames(typeof(Position)).Length;
            options = new ConsolePath[count];

            options[(int)Position.Main] = new ConsolePath(null, new string[]
            {
                "1. Pracownicy",
                "2. Urządzenia",
                "3. Historia urządzeń"
            });

            options[(int)Position.Employees] = new ConsolePath(new string[] {"Pracownicy"}, new string[]
            {
                "1. Lista pracowników",
                "2. Dodaj pracownika",
                "3. Edytuj pracownika",
                "4. Usuń pracownika"
            });

            options[(int)Position.Devices] = new ConsolePath(new string[] { "Urządzenia" }, new string[]
            {
                "1. Dodaj nowe urządzenie",
                "2. Edytuj urządzenie",
                "3. Usuń urządzenie"
            });

            options[(int)Position.History] = new ConsolePath(new string[] { "Historia" }, new string[]
            {
                "1. Lista historii",
                "2. Dodaj"
            });
        }

        public void Draw(Position pos)
        {
            var option = options[(int)pos];
            var path = option.Path;
            var headerName = "Ewidencja spzętu komputerowego";
            if(path is not null && path.Length > 0)
            {
                var headerPath = "";
                headerPath = $" - {path[0]}";
                if(path.Length > 1)
                {
                    for (int i = 1; i < path.Length; i++) headerPath += $"/{path[i]}";
                }
                headerName += headerPath;
            }
            var headerBorder = new String('-', headerName.Length);


            Console.Clear();
            Console.WriteLine(headerBorder);
            Console.WriteLine(headerName);
            Console.WriteLine(headerBorder);
            Console.WriteLine("");

            for(int i = 0; i < option.Choices.Length; i++)
            {
                Console.WriteLine(option.Choices[i]);
            }
            Console.WriteLine("0. <- Powrót");
        }
    }
}
