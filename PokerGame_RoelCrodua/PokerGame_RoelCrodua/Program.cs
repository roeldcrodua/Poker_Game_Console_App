using System;

namespace PokerGame_RoelCrodua
{
    class Program
    {
        static void Main()
        {
            PokerApp poker = new PokerApp();

            //Trigger to run the Official Poker Game.
            //poker.PlayPoker();

            //Testing and recording how the possibility of getting the best hand.
            //Called inside PlayPoker.
            //poker.TestHands();

            // Run poker game automatically for how many times being specified.
            poker.TestRuns();

            //Testing one hand manually by assigning the appropriate card values.
            //poker.TestAHand();
        }
    }
}
