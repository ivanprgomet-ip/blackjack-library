using BlackjackLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLibrary.Internal.Extensions
{
   public static class CardExtensions
    {
        public static string GetRankString(this CardRank rank)
        {
            switch (rank)
            {
                case CardRank.ace:
                case CardRank.two:
                case CardRank.three:
                case CardRank.four:
                case CardRank.five:
                case CardRank.six:
                case CardRank.seven:
                case CardRank.eight:
                case CardRank.nine:
                case CardRank.ten:
                    return ((int)rank).ToString();
                case CardRank.jack:
                    return "j";
                case CardRank.queen:
                    return "q";
                case CardRank.king:
                    return "k";
                default:
                    return "";
            }
        }
    }
}
