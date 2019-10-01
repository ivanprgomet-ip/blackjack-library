using BlackjackLibrary.API;
using BlackjackLibrary.Models;
using System.Collections.Generic;
using System.Reflection;

namespace BlackjackLibrary.Enums
{
    internal static class BlackjackExtensions
    {
        public static int GetBlackjackHandScore(this List<Card> hand)
        {
            return new BlackjackRegulations().CalculateHandValue(hand);
        }
    }
}
