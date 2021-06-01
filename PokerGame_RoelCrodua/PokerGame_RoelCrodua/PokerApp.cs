using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace PokerGame_RoelCrodua
{
    class PokerApp
    {
        public static string player1;
        public static string player2;
        public static int playTimes = 0;

        //Trigger to run the Official Poker Game.
        public void PlayPoker()
        {
            UserInterface ui = new UserInterface();
            bool playing = true;
            string playAgain;

            string answer = ui.PromptForString("Want to test first the possiblity of getting the best hand? [y/n]\t");
            if (answer.ToLower() == "y")
                TestHands();
            else
                Console.WriteLine("Skipping the test of hands!");

            player1 = ui.PromptForString("Enter name of Player1:\t");
            player2 = ui.PromptForString("Enter name of Player2:\t");
            Statistics.gameStat += $"Poker Game was executed at {DateTime.Now}\n";

            while (playing)
            {
                playTimes++;
                PlayAHand();
                ui.EmptyLine();

                playAgain = ui.PromptForString("\nPlay again? [y/n]\t");
                while (playAgain != "Y" && playAgain != "N")
                {
                    playAgain = ui.PromptForString("Invalid answer! [y/n] only:\t");
                }
                if (playAgain == "N")
                    break;
            }
            Statistics.GameReport();
            Statistics.CreateGameFileStat();
        }

        //Called by the poker game.
        private static void PlayAHand()
        {
            UserInterface ui = new UserInterface();
            CardDeck deck = new CardDeck();
            deck.Shuffle();

            Card[] player1hand = new Card[5];
            Card[] player2hand = new Card[5];

            for (int i = 0; i < player1hand.Length; i++)
                player1hand[i] = deck.Deal();

            for (int i = 0; i < player2hand.Length; i++)
                player2hand[i] = deck.Deal();


            PokerHand ph = new PokerHand();
            ui.DisplayLine($"\n{PokerApp.player1} Hand:");
            CardDeck.DisplayCards(player1hand);
            CardDeck.DisplayShortCards(player1hand);
            WinningHands player1Score = ph.ScoreHand(player1hand);
            ui.DisplayLine($"WINNING HAND VALUE: {player1Score}");
            ui.DisplayLine("==================");
            ui.DisplayLine($"\n{PokerApp.player2} Hand:");
            Statistics.gameStat += "\t";
            CardDeck.DisplayCards(player2hand);
            CardDeck.DisplayShortCards(player2hand);
            WinningHands player2Score = ph.ScoreHand(player2hand);
            ui.DisplayLine($"WINNING HAND VALUE: {player2Score}");
            ui.DisplayLine("==================");

            ph.Compare(player1hand, player2hand);
            ui.DisplayLine("\n=================================================");
            ui.DisplayLine("+=+=+=+=+=+=+=R=E=S=U=L=T=S=+=+=+=+=+=+=+=+=+=+=+");
            ui.DisplayLine("=================================================");
            ui.DisplayLine($"{PokerHand.winningMessage}");
            ph.PrintAllCards(ph.SortCard(player1hand), ph.SortCard(player2hand));

            Statistics.gameStat += $"\t{player1Score,10}\t{player2Score,10}";
            Statistics.GameStat();
            PokerHand.winningMessage = "";
        }

        //Testing and recording how the possibility of getting the best hand.
        public void TestHands()
        {
            Statistics stats = new Statistics();
            PokerHand h = new PokerHand();
            UserInterface ui = new UserInterface();

            int test = int.Parse(ui.PromptForString("Enter how many test of the hands you want to execute?\t"));
            Statistics.gameStat += $"Testing of hands executed at {DateTime.Now}\n";
            for (int i = 0; i <= test; i++)
            {
                CardDeck d = new CardDeck();
                d.Shuffle();
                Card[] handTest = new Card[5];

                for (int x = 0; x < handTest.Length; x++)
                    handTest[x] = d.Deal();

                WinningHands ps = h.ScoreHand(handTest);
                stats.Add(ps);
            }
            Console.WriteLine();
            stats.playCount = test;
            stats.TestReport();
        }

        // Run poker game automatically for how many times being specified.
        public void TestRuns()
        {
            UserInterface ui = new UserInterface();

            string answer = ui.PromptForString("Want to test first the possiblity of getting the best hand? [y/n]\t");
            if (answer.ToLower() == "y")
                TestHands();
            else
                Console.WriteLine("Skipping the test of hands!");

            int playing = ui.PromptForInt("Enter how many times you want some test execution:\t");

            player1 = ui.PromptForString("Enter name of Player1:\t");
            player2 = ui.PromptForString("Enter name of Player2:\t");
            Statistics.gameStat += $"GAME PLAYED at {DateTime.Now}\n";
            int test = 0;
            while (test < playing)
            {
                playTimes++;
                PlayAHand();
                ui.EmptyLine();
                test++;
            }
            Statistics.GameReport();
            Statistics.CreateGameFileStat();
        }

        //Testing one hand manually by assigning the appropriate card values.
        public void TestAHand()
        {
            CardDeck deck = new CardDeck();
            deck.Shuffle();

            Card[] player1hand =
                {
                new Card(0, 2),
                new Card(2, 3),
                new Card(5, 3),
                new Card(7, 0),
                new Card(8, 2)
                };

            Card[] player2hand =
                {
                new Card(0, 1),
                new Card(3, 2),
                new Card(5, 2),
                new Card(7, 1),
                new Card(8, 0)
                };

            UserInterface ui = new UserInterface();
            PokerHand ph = new PokerHand();

            ui.DisplayLine($"\n{PokerApp.player1} Hand:");
            WinningHands player1Score = ph.ScoreHand(player1hand);
            ui.DisplayLine($"WINNING HAND VALUE: {player1Score}");
            ui.DisplayLine("==================");

            ui.DisplayLine($"\n{PokerApp.player2} Hand:");
            WinningHands player2Score = ph.ScoreHand(player2hand);
            ui.DisplayLine($"WINNING HAND VALUE: {player2Score}");
            ui.DisplayLine("==================");

            ph.Compare(player1hand, player2hand);
            ui.DisplayLine("\n=================================================");
            ui.DisplayLine("+=+=+=+=+=+=+=R=E=S=U=L=T=S=+=+=+=+=+=+=+=+=+=+=+");
            ui.DisplayLine("=================================================");
            ui.DisplayLine($"{PokerHand.winningMessage}");
            ph.PrintAllCards(ph.SortCard(player1hand), ph.SortCard(player2hand));
        }
    }
}

