using System;
using System.Collections.Generic;
using System.Text;

namespace PokerGame_RoelCrodua
{
    class UserInterface
    {
        public void EmptyLine() => Console.WriteLine();
        public void DisplayLine(string message) => Console.WriteLine(message);
        public void Display(string message) => Console.Write(message);
        public string PromptForString(string message)
        {
            Display(message);
            return Console.ReadLine().ToUpper();
        }
        public int PromptForInt(string message)
        {
            Display(message);
            return int.Parse(Console.ReadLine());
        }
    }
}
