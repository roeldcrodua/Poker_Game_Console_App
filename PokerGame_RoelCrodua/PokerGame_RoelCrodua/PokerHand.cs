using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerGame_RoelCrodua
{
    public enum WinningHands
    {
        FiveOfAKind,
        RoyalFlush,
        StraightFlush,
        FourOfAKind,
        FullHouse,
        Flush,
        Straight,
        ThreeOfAKind,
        TwoPair,
        Pair,
        HighCard,
        NotScored
    }

    class PokerHand
    {
        public static int pairFace1 = 0;
        public static int pairFace2 = 0;
        public static string winningMessage = string.Empty;
        public static string cardWinner = string.Empty;

        private WinningHands score = WinningHands.NotScored;
        public WinningHands Score
        {
            get { return score; }
            private set
            {
                if (value < score)
                    score = value;
                else if (value == WinningHands.NotScored)
                    score = value;
            }
        }

        public string Compare(Card[] hand1, Card[] hand2)
        {
            WinningHands score1 = ScoreHand(hand1);
            WinningHands score2 = ScoreHand(hand2);

            if (score1 < score2)
                winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(hand1)}] wins over {PokerApp.player2}'s [{ScoreOfWhat(hand2)}]";
            else if (score1 > score2)
                winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(hand1)}] lost over {PokerApp.player2}'s [{ScoreOfWhat(hand2)}]";
            else
                winningMessage = $"{TieBreaker(hand1, hand2)}";
            return winningMessage;
        }

        public Card[] SortCard(Card[] hand)
        {
            // Sorting the values of each Hands.

            Card[] Hands = new Card[5];

            var sortCardHand =
                from card in hand
                orderby card.Face
                select card;

            int c = 0;
            foreach (Card cards in sortCardHand)
            {
                for (int i = 0; i < 5; i++)
                    if (i == c)
                        Hands[c] = cards;
                c++;
            }
            return Hands;
        }

        public string TieBreaker(Card[] Hand1, Card[] Hand2)
        {
            // Sort the card to make them arranged from [0] as lowest to [4] as the highest face card.
            Card[] Hands1 = SortCard(Hand1);
            Card[] Hands2 = SortCard(Hand2);

            int totalHandCard1 = 0;
            int totalHandCard2 = 0;

            int c1 = 0;
            foreach (Card cards1 in Hands1)
                totalHandCard1 += Hands1[c1++].Face;

            int c2 = 0;
            foreach (Card cards2 in Hands2)
                totalHandCard2 += Hands2[c2++].Face;

            switch (Score)
            {
                //FiveOfAKind == add the face values
                //StraightFlush == add the face values
                //FourOfAKind == add the face values
                //Straight == add the face values
                case WinningHands.FiveOfAKind:
                case WinningHands.FourOfAKind:
                case WinningHands.StraightFlush:
                case WinningHands.Straight:
                    {
                        if (totalHandCard1 < totalHandCard2)
                            winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] wins over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}]";
                        else if (totalHandCard1 > totalHandCard2)
                            winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] lost over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}]";
                        else
                            winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] == {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}]";
                        break;
                    }
                //ThreeOfAKind == compare the 4th then 2nd card for each player, if still match then compare the next higher, then the next to the last card
                //FullHouse == compare the 4th then 2nd card for each player, if still match then compare the next higher, then the next to the last card
                case WinningHands.ThreeOfAKind:
                case WinningHands.FullHouse:
                    {
                        if (Hands1[2].Face > Hands2[2].Face || Hands1[0].FaceName == "Ace")
                            winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] wins over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}]";
                        else if (Hands1[2].Face < Hands2[2].Face || Hands2[0].FaceName == "Ace")
                            winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] lost over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}]";
                        else
                            winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] == {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}]";
                        break;
                    }
                //TwoPair == compare the highest pair [3], then the next pair [1], then the next higher to the lowest card [4] [2] [1] [0]
                case WinningHands.TwoPair:
                    {
                        if (Hands1[3].Face == Hands2[3].Face)
                        {
                            winningMessage = $"{PokerApp.player1} and {PokerApp.player2} tie 1";
                            if ((Hands1[1].Face == Hands2[1].Face) || (Hands1[1].FaceName == "Ace" && Hands2[1].FaceName == "Ace"))
                            {
                                winningMessage = $"{PokerApp.player1} and {PokerApp.player2} tie 2";
                                if (Hands1[4].Face == Hands2[4].Face)
                                {
                                    winningMessage = $"{PokerApp.player1} and {PokerApp.player2} tie 3";
                                    if (Hands1[2].Face == Hands2[2].Face)
                                    {
                                        winningMessage = $"{PokerApp.player1} and {PokerApp.player2} tie 4";
                                        if ((Hands1[0].FaceName == "Ace" && Hands2[0].FaceName == "Ace") || Hands1[0].Face == Hands2[0].Face)
                                            winningMessage = $"{PokerApp.player1} 5 Cards matches to {PokerApp.player2} 5 Cards";
                                        else if (Hands1[0].Face > Hands2[0].Face)
                                        {
                                            if (Hands2[0].FaceName != "Ace")
                                                winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] wins over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player1}'s ({Hands1[0].FaceName})";
                                            else if (Hands2[0].FaceName == "Ace")
                                                winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] lost over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player2}'s ({Hands2[0].FaceName})";
                                        }
                                        else if (Hands1[0].Face < Hands2[0].Face)
                                        {
                                            if (Hands1[0].FaceName == "Ace")
                                                winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] wins over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player1}'s ({Hands1[0].FaceName})";
                                            else
                                                winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] lost over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player2}'s ({Hands1[0].FaceName})";
                                        }
                                    }
                                    else if (Hands1[2].Face > Hands2[2].Face)
                                        winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] wins over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player1}'s ({Hands1[2].FaceName})";
                                    else if (Hands1[2].Face < Hands2[2].Face)
                                        winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] lost over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player2}'s ({Hands2[2].FaceName})";
                                }
                                else if (Hands1[4].Face > Hands2[4].Face)
                                    winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] wins over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player1}'s ({Hands1[4].FaceName})";
                                else if (Hands1[4].Face < Hands1[4].Face)
                                    winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] lost over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player2}'s ({Hands2[4].FaceName})";
                            }
                            else if (Hands1[1].Face > Hands2[1].Face)
                            {
                                if (Hands2[1].FaceName != "Ace")
                                    winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] wins over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player1}'s ({Hands1[1].FaceName})";
                                else if (Hands2[1].FaceName == "Ace")
                                    winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] lost over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player2}'s ({Hands2[1].FaceName})";
                            }
                            else if (Hands1[1].Face < Hands2[1].Face)
                            {
                                if (Hands1[1].FaceName == "Ace")
                                    winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] wins over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player1}'s ({Hands1[1].FaceName})";
                                else
                                    winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] lost over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player2}'s ({Hands2[1].FaceName})";
                            }
                        }
                        else if (Hands1[3].Face > Hands2[3].Face || Hands1[1].FaceName == "Ace")
                            winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] wins over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player1}'s ({Hands1[3].FaceName})";
                        else if (Hands1[3].Face < Hands2[3].Face || Hands2[1].FaceName == "Ace")
                            winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] lost over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player2} {Hands2[3].FaceName})";
                        break;
                    }
                //Pair == compare each pair which is highest, then the next highest face up to lowest face
                case WinningHands.Pair:
                    {
                        if (Hands1[0].Face == Hands1[1].Face)
                            pairFace1 = Hands1[0].Face;
                        else if (Hands1[1].Face == Hands1[2].Face)
                            pairFace1 = Hands1[1].Face;
                        else if (Hands1[2].Face == Hands1[3].Face)
                            pairFace1 = Hands1[2].Face;
                        else if (Hands1[3].Face == Hands1[4].Face)
                            pairFace1 = Hands1[3].Face;

                        if (Hands2[0].Face == Hands2[1].Face)
                            pairFace2 = Hands2[0].Face;
                        else if (Hands2[1].Face == Hands2[2].Face)
                            pairFace2 = Hands2[1].Face;
                        else if (Hands2[2].Face == Hands2[3].Face)
                            pairFace2 = Hands2[2].Face;
                        else if (Hands2[3].Face == Hands2[4].Face)
                            pairFace2 = Hands2[3].Face;

                        if (pairFace1 == pairFace2)
                        {
                            if (Hands1[4].Face == Hands2[4].Face)
                            {
                                winningMessage = $"{PokerApp.player1} and {PokerApp.player2} tie 1";
                                if (Hands1[3].Face == Hands2[3].Face)
                                {
                                    winningMessage = $"{PokerApp.player1} and {PokerApp.player2} tie 2";
                                    if (Hands1[2].Face == Hands2[2].Face)
                                    {
                                        winningMessage = $"{PokerApp.player1} and {PokerApp.player2} tie 3";
                                        if (Hands1[1].Face == Hands2[1].Face)
                                        {
                                            winningMessage = $"{PokerApp.player1} and {PokerApp.player2} tie 4";
                                            if ((Hands1[0].FaceName == "Ace" && Hands2[0].FaceName == "Ace") || Hands1[0].Face == Hands2[0].Face)
                                                winningMessage = $"{PokerApp.player1} 5 Cards matches to {PokerApp.player2} 5 Cards";
                                            else if (Hands1[0].Face > Hands2[0].Face)
                                            {
                                                if (Hands2[0].FaceName != "Ace")
                                                    winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] wins over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player1}'s ({Hands1[0].FaceName})";
                                                else if (Hands2[0].FaceName == "Ace")
                                                    winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] lost over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player2}'s ({Hands2[0].FaceName})";
                                            }
                                            else if (Hands1[0].Face < Hands2[0].Face)
                                            {
                                                if (Hands1[0].FaceName == "Ace")
                                                    winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] wins over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player1}'s ({Hands1[0].FaceName})";
                                                else
                                                    winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] lost over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player2}'s ({Hands2[0].FaceName})";
                                            }
                                        }
                                        else if (Hands1[1].Face > Hands2[1].Face)
                                            winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] wins over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player1}'s ({Hands1[1].FaceName})";
                                        else if (Hands1[1].Face < Hands2[1].Face)
                                            winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] lost over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player2}'s ({Hands2[1].FaceName})";
                                    }
                                    else if (Hands1[2].Face > Hands2[2].Face)
                                        winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] wins over {PokerApp.player2} [{ScoreOfWhat(Hand2)} because of {PokerApp.player1}'s ({Hands1[2].FaceName}]";
                                    else if (Hands1[2].Face < Hands1[2].Face)
                                        winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] lost over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player2}'s ({Hands2[2].FaceName})";
                                }
                                else if (Hands1[3].Face > Hands2[3].Face)
                                    winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] wins over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player1}'s ({Hands1[3].FaceName})";
                                else if (Hands1[3].Face < Hands2[3].Face)
                                    winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] lost over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player2}'s ({Hands2[3].FaceName})";
                            }
                            else if (Hands1[4].Face > Hands2[4].Face)
                                winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] wins over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player1}'s ({Hands1[4].FaceName})";
                            else if (Hands1[4].Face < Hands2[4].Face)
                                winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] lost over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player2}'s ({Hands2[4].FaceName})";
                        }
                        else if (pairFace1 > pairFace2)
                        {
                            if (Card.FacesNames[pairFace2] != "Ace")
                                winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] wins over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player1}'s ({Card.FacesNames[pairFace1]})";
                            else if (Card.FacesNames[pairFace2] == "Ace")
                                winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] lost over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player2}'s ({Card.FacesNames[pairFace2]})";
                        }
                        else if (pairFace1 < pairFace2)
                        {
                            if (Card.FacesNames[pairFace1] == "Ace")
                                winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] wins over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player1}'s ({Card.FacesNames[pairFace1]})";
                            else
                                winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] lost over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player2}'s ({Card.FacesNames[pairFace2]})";
                        }
                        break;
                    }
                //Flush == compare each card from highest face to lowest face card
                //HighCard == no pair but compare from the highest face to lowest face card
                case WinningHands.Flush:
                case WinningHands.HighCard:
                    {
                        if ((Hands1[0].FaceName != "Ace" && Hands2[0].FaceName != "Ace") || (Hands1[0].FaceName == "Ace" && Hands2[0].FaceName == "Ace"))
                        {
                            if (Hands1[4].Face == Hands2[4].Face)
                            {
                                winningMessage = $"{PokerApp.player1} and {PokerApp.player2} tie 1";
                                if (Hands1[3].Face == Hands2[3].Face)
                                {
                                    winningMessage = $"{PokerApp.player1} and {PokerApp.player2} tie 2";
                                    if (Hands1[2].Face == Hands2[2].Face)
                                    {
                                        winningMessage = $"{PokerApp.player1} and {PokerApp.player2} tie 3";
                                        if (Hands1[1].Face == Hands2[1].Face)
                                        {
                                            winningMessage = $"{PokerApp.player1} and {PokerApp.player2} tie 4";
                                            if ((Hands1[0].FaceName == "Ace" && Hands2[0].FaceName == "Ace") || Hands1[0].Face == Hands2[0].Face)
                                                winningMessage = $"{PokerApp.player1} 5 Cards matches to {PokerApp.player2} 5 Cards";
                                            else if (Hands1[0].Face > Hands2[0].Face)
                                            {
                                                if (Hands2[0].FaceName != "Ace")
                                                    winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] wins over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player1}'s ({Hands1[0].FaceName})";
                                                else if (Hands2[0].FaceName == "Ace")
                                                    winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] lost over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player2}'s ({Hands2[0].FaceName})";
                                            }
                                            else if (Hands1[0].Face < Hands2[0].Face)
                                            {
                                                if (Hands1[0].FaceName == "Ace")
                                                    winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] wins over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player1}'s ({Hands1[0].FaceName})";
                                                else
                                                    winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] lost over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player2}'s ({Hands2[0].FaceName})";
                                            }
                                        }
                                        else if (Hands1[1].Face > Hands2[1].Face)
                                            winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] wins over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player1}'s ({Hands1[1].FaceName})";
                                        else if (Hands1[1].Face < Hands2[1].Face)
                                            winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] lost over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player2}'s ({Hands2[1].FaceName})";
                                    }
                                    else if (Hands1[2].Face > Hands2[2].Face)
                                        winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] wins over {PokerApp.player2} [{ScoreOfWhat(Hand2)} because of {PokerApp.player1}'s ({Hands1[2].FaceName}]";
                                    else if (Hands1[2].Face < Hands1[2].Face)
                                        winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] lost over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player2}'s ({Hands2[2].FaceName})";
                                }
                                else if (Hands1[3].Face > Hands2[3].Face)
                                    winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] wins over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player1}'s ({Hands1[3].FaceName})";
                                else if (Hands1[3].Face < Hands2[3].Face)
                                    winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] lost over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player2}'s ({Hands2[3].FaceName})";
                            }
                            else if (Hands1[4].Face > Hands2[4].Face)
                                winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] wins over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player1}'s ({Hands1[4].FaceName})";
                            else if (Hands1[4].Face < Hands2[4].Face)
                                winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] lost over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player2}'s ({Hands2[4].FaceName})";
                        }

                        if ((Hands1[0].FaceName == "Ace" && Hands2[0].FaceName != "Ace") || (Hands1[0].FaceName != "Ace" && Hands2[0].FaceName == "Ace"))
                            {
                                if (Hands1[0].FaceName == "Ace" && Hands2[0].FaceName != "Ace")
                                    winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] wins over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player1}'s ({Hands1[0].FaceName})";
                                else if (Hands1[0].FaceName != "Ace" && Hands2[0].FaceName == "Ace")
                                    winningMessage = $"{PokerApp.player1}'s [{ScoreOfWhat(Hand1)}] lost over {PokerApp.player2}'s [{ScoreOfWhat(Hand2)}] because of {PokerApp.player2}'s ({Hands2[0].FaceName})";
                        }
                        
                        break;
                    }
                //RoyalFlush == tie because it consist only a unique card combination.
                case WinningHands.RoyalFlush:
                    {
                        winningMessage = $"{PokerApp.player1} and  {PokerApp.player2} got Tie.";
                        break;
                    }
            }
            return winningMessage;
        }

        public string ScoreOfWhat(Card[] hand)
        {
            Card[] sortHand = SortCard(hand);
            string winnerCard = string.Empty;

            switch (ScoreHand(hand))
            {
                case WinningHands.FiveOfAKind:
                case WinningHands.FourOfAKind:
                    winnerCard = $"{Score} of {(sortHand[0].FaceName != "Ace" ? sortHand[2].FaceName : sortHand[0].FaceName)}";
                    break;
                case WinningHands.StraightFlush:
                case WinningHands.Flush:
                    winnerCard = $"{Score} of {sortHand[2].SuitName} ({(sortHand[0].FaceName != "Ace" ? sortHand[4].FaceName : sortHand[0].FaceName)} as highest)";
                    break;
                case WinningHands.Straight:
                    winnerCard = $"{Score} of {(sortHand[0].FaceName != "Ace" ? sortHand[4].FaceName : sortHand[0].FaceName)} as highest";
                    break;
                case WinningHands.ThreeOfAKind:
                    winnerCard = $"{Score} of {sortHand[2].FaceName} with ({(sortHand[0].FaceName != "Ace" ? sortHand[4].FaceName : sortHand[0].FaceName)} as highest)";
                    break;
                case WinningHands.FullHouse:
                    winnerCard = $"{Score} of {sortHand[2].FaceName} & {(sortHand[2].Face == sortHand[1].Face ? sortHand[3].FaceName : sortHand[1].FaceName)}";
                    break;
                case WinningHands.TwoPair:
                    winnerCard = $"{Score} of {sortHand[3].FaceName} & {sortHand[1].FaceName})";
                    break;
                case WinningHands.Pair:
                    {
                        winnerCard = $"{Score} of {(sortHand[1].Face == sortHand[0].Face ? sortHand[1].FaceName : "")}{(sortHand[1].Face == sortHand[2].Face ? sortHand[1].FaceName : "")}" +
                                             $"{(sortHand[3].Face == sortHand[2].Face ? sortHand[3].FaceName : "")}{(sortHand[3].Face == sortHand[4].Face ? sortHand[3].FaceName : "")}";
                        break;
                    }
                case WinningHands.HighCard:
                    winnerCard = $"{Score} of {(sortHand[0].FaceName != "Ace" ? sortHand[4].FaceName : sortHand[0].FaceName)}";
                    break;
                case WinningHands.RoyalFlush:
                    winnerCard = $"{Score}";
                    break;
            }
            return winnerCard;
        }

        public WinningHands ScoreHand(Card[] hand)
        {
            Score = WinningHands.NotScored;
            int[] countOfFaces = new int[Card.Faces.Length];
            int[] countOfSuits = new int[Card.Suits.Length];
            for (int card = 0; card < hand.Length; card++)
            {
                countOfFaces[hand[card].Face]++;
                countOfSuits[hand[card].Suit]++;
            }

            // Check for Flush
            for (int suit = 0; suit < countOfSuits.Length; suit++)
            {
                if (countOfSuits[suit] == hand.Length)
                {
                    Score = WinningHands.Flush;
                    break;
                }

            }

            for (int face = 0; face < countOfFaces.Length; face++)
            {
                switch (countOfFaces[face])
                {
                    case 5:
                        {
                            Score = WinningHands.FiveOfAKind;
                            break;
                        }
                    case 4:
                        {
                            Score = WinningHands.FourOfAKind;
                            break;
                        }
                    case 3:
                        if (Score == WinningHands.Pair)
                            Score = WinningHands.FullHouse;
                        else
                        {
                            Score = WinningHands.ThreeOfAKind;
                        }
                        break;
                    case 2:
                        if (Score == WinningHands.Pair)
                        {
                            Score = WinningHands.TwoPair;
                        }
                        else
                        {
                            if (Score == WinningHands.ThreeOfAKind)
                            {
                                Score = WinningHands.FullHouse;
                            }
                            else
                            {
                                Score = WinningHands.Pair;
                            }
                        }
                        break;
                    case 1:
                        {
                            Score = WinningHands.HighCard;
                            break;
                        }
                }
            }
            var runLength = 0;
            for (int face = 0; face < countOfFaces.Length; face++)
            {
                if (runLength == 5)
                    break;

                if (countOfFaces[face] == 1)
                    runLength++;
                else
                    runLength = 0;
            }
            if (runLength == 5)
            {
                if (Score == WinningHands.Flush)
                    Score = WinningHands.StraightFlush;
                else
                    Score = WinningHands.Straight;
            }
            if (runLength == 4)
            {
                if (countOfFaces[9] == 1 && countOfFaces[12] == 1 && countOfFaces[0] == 1)
                {
                    if (Score == WinningHands.Flush)
                        Score = WinningHands.RoyalFlush;
                    else
                        Score = WinningHands.Straight;
                }
            }


            return Score;
        }

        public void PrintAllCards(Card[] Hand1, Card[] Hand2)
        {
            Console.WriteLine($"\n{PokerApp.player1}{PokerApp.player2,30}");
            Console.WriteLine("=================================================");
            for (int i = 0; i < Hand1.Length; i++)
                Console.WriteLine(string.Format($"{Hand1[i],15}{Hand2[i],25}"));
        }
    }
}
