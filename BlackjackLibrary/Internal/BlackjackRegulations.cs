using BlackjackLibrary.Enums;
using BlackjackLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
namespace BlackjackLibrary.Models
{
    internal class BlackjackRegulations : IBlackjackRegulations
    {
        public bool IsBust(IPlayer player)
        {
            if (CalculateHandValue(player.Hand) > 21)
                return true;
            return false;
        }

        public Winninghand EvaluateWinner(List<Card> playerHand, List<Card> dealerHand)
        {
            int playerHandValue = CalculateHandValue((playerHand));
            int dealerHandValue = CalculateHandValue((dealerHand));

            if (playerHandValue > 21)
                return dealerHandValue > 21 ? Winninghand.Draw : Winninghand.Dealer;
            else if (dealerHandValue > 21)
                return playerHandValue > 21 ? Winninghand.Draw : Winninghand.Player;
            else
            {
                if (dealerHandValue == playerHandValue)
                    return Winninghand.Draw;
                else
                    return playerHandValue > dealerHandValue ? Winninghand.Player : Winninghand.Dealer;
            }
        }

        public int CalculateHandValue(List<Card> hand)
        {
            var sum = 0;

            //goes over all cards in the hand and first adds 
            //non-ace cards and then adds the ace cards at the end
            List<Card> TempHand = new List<Card>();
            foreach (var card in hand)
            {
                if (card.Rank != CardRank.ace)
                    TempHand.Add(card);
            }

            foreach (var card in hand)
            {
                if (card.Rank == CardRank.ace)
                    TempHand.Add(card);
            }

            //having all ace cards (if any) at the end 
            //makes the estimation algorithm below work
            foreach (var card in TempHand)
            {
                // hidden cards are not included in calculation
                if (card.IsHidden)
                    sum += 0;
                else
                {
                    if (card.Rank >= CardRank.two && card.Rank <= CardRank.ten)
                        sum += (int)card.Rank;
                    // Faces (J, Q, K) have value 10
                    else if (card.Rank > CardRank.ten)
                        sum += 10;

                    // Decide if an Ace is worth 1 or 11
                    else if (card.Rank == CardRank.ace)
                    {
                        // This gives Black Jack (sum = 21) 
                        // and counts towards highest possible value
                        if (sum <= 10)
                            sum += 11;
                        else
                            sum += 1;
                    }
                }
            }
            return sum;
        }

        public bool IsBust(List<Card> hand)
        {
            if (CalculateHandValue(hand) > 21)
                return true;
            return false;
        }

        public bool IsBlackjack(List<Card> hand)
        {
            if (CalculateHandValue(hand) == 21)
                return true;
            return false;
        }

        public HandStatus GetHandStatus(List<Card> hand)
        {
            if (IsBust(hand))
                return HandStatus.bust;
            else if (IsBlackjack(hand) && hand.Count == 2)
                return HandStatus.Natural;
            else if (IsBlackjack(hand))
                return HandStatus.blackjack;
            else
                return HandStatus.numeric;
        }
    }
}