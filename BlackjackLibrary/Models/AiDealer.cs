using System;
using System.Collections.Generic;
using BlackjackLibrary.Interfaces;
using BlackjackLibrary.Enums;
using BlackjackLibrary.EventArgs;

namespace BlackjackLibrary.Models
{
    public class AiDealer : PlayerBase, IDealer
    {
        /// <summary>
        /// Occurs whenever a card has been dealt to someone
        /// </summary>
        public event EventHandler<CardDealtEventArgs> CardDealt;
        public AiDealer(string name, decimal balance) : base(name, balance) { }
        public override BlackjackDecision MakeMove(Func<List<Card>, int> calcHandValue)
        {
            if (calcHandValue(Hand) >= 17)
                return BlackjackDecision.stand;
            return BlackjackDecision.hit;
        }
        public BlackjackDecision ComputeDecision(int handValue)
        {
            if (handValue >= 17)
                return BlackjackDecision.stand;
            return BlackjackDecision.hit;
        }
        public List<Card> DealTo(IPlayer player, ref int count, Deck<Card> deck, Func<List<Card>,HandStatus> getStatus)
        {
            if (count == 0)
                return player.Hand;
            else
            {
                var card = deck.OneFromTop();
                player.Hand.Add(card);
                player.Status = getStatus(player.Hand);
                CardDealt?.Invoke(this, new CardDealtEventArgs(player.Id, card, player.Hand.Count - 1));
                count -= 1;
                return DealTo(player, ref count, deck, getStatus);
            }
        }
        public List<Card> DealTo(IPlayer player, ref int count, Deck<Card> deck, bool isHidden, Func<List<Card>, HandStatus> getStatus)
        {
            if (count == 0)
                return player.Hand;
            else
            {
                var card = deck.OneFromTop();
                card.IsHidden = isHidden;
                player.Hand.Add(card);
                player.Status = getStatus(player.Hand);
                CardDealt?.Invoke(this, new CardDealtEventArgs(player.Id, card, player.Hand.Count - 1));
                count -= 1;
                return DealTo(player, ref count, deck, getStatus);
            }
        }
    }
}