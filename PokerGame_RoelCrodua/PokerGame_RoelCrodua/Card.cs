using System;
using System.Collections.Generic;
using System.Text;


namespace PokerGame_RoelCrodua
{
    class Card
    {

        public readonly static string[] FacesNames = { "Ace", "Two", "Three", "Four", "Five", "Six",
                                                        "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King"};
        public readonly static string[] SuitsNames = { "Diamonds", "Clubs", "Hearts", "Spades" };

        public readonly static string[] Faces = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

        public readonly static string[] Suits = { "D", "C", "H", "S" };

        private int face;
        public int Face
        {
            get { return face; }
            private set
            {
                if (value < 0 || value > Faces.Length)
                    throw new Exception("Invalid value for Face");
                face = value;
            }
        }

        private int suit;
        public int Suit
        {
            get { return suit; }
            private set
            {
                if (value < 0 || value > Suits.Length)
                    throw new Exception("Invalid value for Suit");
                suit = value;

            }
        }

        public string FaceName
        {
            get { return FacesNames[Face]; }
        }
        public string SuitName
        {

            get { return SuitsNames[Suit]; }
        }

        public Card(int face, int suit)
        {
            Face = face;
            Suit = suit;
        }

        public override string ToString()
        {

            return $"{FaceName} {SuitName}";
        }

        public static string DisplayShortCard(int Face, int Suit)
        {
            return $"{Faces[Face]}{Suits[Suit]}";
        }
    }
}
