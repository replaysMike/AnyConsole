using System.Drawing;
using System.Threading.Tasks;

namespace AnyConsole.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var console = new ExtendedConsole(new ConsoleOptions(RenderOptions.FadeHistory | RenderOptions.HideCursor, InputOptions.UseBuiltInKeyOperations));
            console.Configure(config =>
            {
                config.SetStaticRow("Header", RowLocation.Top, Color.White, Color.DarkRed);
                config.SetStaticRow("SubHeader", RowLocation.Top, 1, Color.White, Color.FromArgb(30, 30, 30));
                config.SetStaticRow("Footer", RowLocation.Bottom, Color.White, Color.DarkBlue);
                config.SetLogHistoryContainer(RowLocation.Top, 2);
                // todo: build frame dimensions into static rows
                //config.SetWindowFrame(Color.FromArgb(200, 200, 30), 1);
            });
            console.OnKeyPress += Console_OnKeyPress;
            console.WriteRow("Header", "Game Server", ColumnLocation.Left, Color.Yellow);
            console.WriteRow("Header", Component.Time, ColumnLocation.Right);
            console.WriteRow("SubHeader", "This is a test application console", ColumnLocation.Left, Color.FromArgb(60, 60, 60));
            console.Start();

            console.WriteLine("FIRST FIRST FIRST FIRST");
            console.WriteLine("Some text to render");
            console.WriteLine("Once upon a time there was a little boy");
            console.WriteLine("and all the christmas elves shouted for joy");
            console.WriteLine("Jack and Jill went up a hill to fetch a pail of water.");
            console.WriteLine("Some text to render");
            console.WriteLine("Once upon a time there was a little boy");
            console.WriteLine("and all the christmas elves shouted for joy");
            console.WriteLine("Jack and Jill went up a hill to fetch a pail of water.");
            console.WriteLine("Some text to render");
            console.WriteLine("Once upon a time there was a little boy");
            console.WriteLine("and all the christmas elves shouted for joy");
            console.WriteLine("Jack and Jill went up a hill to fetch a pail of water.");
            console.WriteLine("Some text to render");
            console.WriteLine("Once upon a time there was a little boy");
            console.WriteLine("and all the christmas elves shouted for joy");
            console.WriteLine("Jack and Jill went up a hill to fetch a pail of water.");
            console.WriteLine("Some text to render");
            console.WriteLine("Once upon a time there was a little boy");
            console.WriteLine("and all the christmas elves shouted for joy");
            console.WriteLine("Jack and Jill went up a hill to fetch a pail of water.");
            console.WriteLine("Some text to render");
            console.WriteLine("Once upon a time there was a little boy");
            console.WriteLine("and all the christmas elves shouted for joy");
            console.WriteLine("Jack and Jill went up a hill to fetch a pail of water.");
            console.WriteLine("Some text to render");
            console.WriteLine("Once upon a time there was a little boy");
            console.WriteLine("and all the christmas elves shouted for joy");
            console.WriteLine("Jack and Jill went up a hill to fetch a pail of water.");
            console.WriteLine("Some text to render");
            console.WriteLine("Once upon a time there was a little boy");
            console.WriteLine("and all the christmas elves shouted for joy");
            console.WriteLine("Jack and Jill went up a hill to fetch a pail of water.");
            console.WriteLine("Some text to render");
            console.WriteLine("Once upon a time there was a little boy");
            console.WriteLine("and all the christmas elves shouted for joy");
            console.WriteLine("Jack and Jill went up a hill to fetch a pail of water.");
            console.WriteLine("Some text to render");
            console.WriteLine("Once upon a time there was a little boy");
            console.WriteLine("and all the christmas elves shouted for joy");
            console.WriteLine("Jack and Jill went up a hill to fetch a pail of water.");
            console.WriteLine("Some text to render");
            console.WriteLine("Once upon a time there was a little boy");
            console.WriteLine("and all the christmas elves shouted for joy");
            console.WriteLine("Jack and Jill went up a hill to fetch a pail of water.");
            console.WriteLine("LAST LAST LAST LAST LAST LAST");

            console.WaitForClose();

        }

        private static void Console_OnKeyPress(KeyPressEventArgs e)
        {
            System.Console.WriteLine($"KEY PRESSED {e.Key}");
        }
    }
}
