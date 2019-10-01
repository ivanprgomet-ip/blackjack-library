using BlackjackLibrary.Enums;
using BlackjackLibrary.Models;
using System.Collections.Generic;

namespace BlackjackLibrary.Interfaces
{
    internal interface IBlackjackRegulations
    {
        bool IsBust(IPlayer player);
        Winninghand EvaluateWinner(List<Card> playerHand, List<Card> dealerHand);
        int CalculateHandValue(List<Card> hand);
        bool IsBust(List<Card> hand);
        bool IsBlackjack(List<Card> hand);
        HandStatus GetHandStatus(List<Card> hand);
    }
}
