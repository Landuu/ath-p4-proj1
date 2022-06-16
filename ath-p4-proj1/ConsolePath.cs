
namespace ath_p4_proj1
{
    internal class ConsolePath
    {
        public string[] Choices { get; set; }
        public string[]? Path { get; set; }
        public bool PrintControls { get; set; }

        public ConsolePath(string[]? path, string[] choices, bool printControls = true)
        {
            Choices = choices;
            Path = path;
            PrintControls = printControls;
        }
    }
}
