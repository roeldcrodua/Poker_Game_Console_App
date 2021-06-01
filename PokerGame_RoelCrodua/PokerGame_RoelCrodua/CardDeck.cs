using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PokerGame_RoelCrodua
{
    class CardDeck
    {
        private Card[] StackOfCards { get; set; }

        private int currentCard;
        private readonly Random _rnd = new Random();

        public CardDeck()
        {
            Intialize();
        }

        public void Intialize()
        {
            StackOfCards = new Card[Card.Faces.Length * Card.Suits.Length];
            currentCard = 0;

            int cardIndex = 0;
            for (int suit = 0; suit < Card.Suits.Length; suit++)
                for (int face = 0; face < Card.Faces.Length; face++)
                    StackOfCards[cardIndex++] = new Card(face, suit);
        }

        public void Shuffle()
        {
            currentCard = 0;
            for (int card = 0; card < StackOfCards.Length; card++)
            {
                int randomIndex = _rnd.Next(0, StackOfCards.Length);
                Card temp = StackOfCards[card];
                StackOfCards[card] = StackOfCards[randomIndex];
                StackOfCards[randomIndex] = temp;
            }
        }

        public Card Deal()
        {
            if (currentCard >= StackOfCards.Length)
            {
                Shuffle();
            }
            return StackOfCards[currentCard++];

        }
        public static void DisplayCards(Card[] CardHand)
        {
            Console.WriteLine("==================");
            for (int i = 0; i < CardHand.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {CardHand[i]}");
            }
            Console.WriteLine("==================");
        }
        public static void DisplayShortCards(Card[] CardHand)
        {
            for (int i = 0; i < CardHand.Length; i++)
                Statistics.gameStat += $" {Card.DisplayShortCard(CardHand[i].Face, CardHand[i].Suit),3}";
        }
    }
}
