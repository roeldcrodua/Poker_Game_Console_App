using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PokerGame_RoelCrodua
{
	class Statistics
	{
		public static string gameStat = string.Empty;
		public static int player1Wins = 0;
		public static int player2Wins = 0;
		public static int player1Loss = 0;
		public static int player2Loss = 0;
		public static int tie = 0;
		public int playCount;
		private readonly int[] count = new int[Enum.GetValues(typeof(WinningHands)).Length];

		public void TestReport()
		{
			gameStat += $"{ "Hand",10}\t{ "Count",10}\t{"Percent",10}\n";
			for (int i = 0; i < count.Length; ++i)
				gameStat += $"{Enum.GetName(typeof(WinningHands), i),-10}\t{count[i],10}\t{count[i] / (double)playCount,10:p4}\n";

			gameStat += $"{"Total Hands",10}\t{ playCount,10}\n\n";
			Console.WriteLine(gameStat);
		}

		public void Add(WinningHands pokerScore)
		{
			count[(int)pokerScore]++;
		}

		public static void GameStat()
		{
			gameStat += $"\t{PokerApp.playTimes,3}";

			if (PokerHand.winningMessage.Contains("wins"))
			{
				gameStat += $"\tWINNER:{PokerApp.player1}\t{PokerHand.winningMessage}";
				player1Wins++;
				player2Loss++;
			}
			else if (PokerHand.winningMessage.Contains("lost"))
			{
				gameStat += $"\tWINNER:{PokerApp.player2}\t{PokerHand.winningMessage}";
				player2Wins++;
				player1Loss++;
			}
			else
			{
				gameStat += $"\tWINNER:TIE\t\t{PokerHand.winningMessage}";
				tie++;
			}
			gameStat += "\n";
		}

		public static void GameReport()
		{
			gameStat += $"\nTOTAL TIMES PLAYED:\t{PokerApp.playTimes}\n" +
						$"{PokerApp.player1} wins {player1Wins}x over {player1Loss} losses\n" +
						$"{PokerApp.player2} wins {player2Wins}x over {player2Loss} losses\n" +
						$"Both {PokerApp.player1} and {PokerApp.player2} tie {tie}x";

			Console.WriteLine(gameStat);
		}

		public static void CreateGameFileStat()
		{
			try
			{
				var file = File.OpenWrite(@"C:\temp\PokerGameResult.txt");
				var writer = new StreamWriter(file);
				writer.Write(gameStat);
				writer.Close();
				file.Close();
			}
			catch(Exception ex)
			{
				Console.WriteLine("Cannot find the file or directory." + ex.Message);
			}
		}

	}
}
