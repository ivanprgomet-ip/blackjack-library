using BlackjackLibrary.Enums;
using BlackjackLibrary.Internal.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BlackjackLibrary.Models
{
    public class Card
    {
        public CardSuite Suite { get; }
        public CardRank Rank { get; }
        public bool IsHidden { get; set; }
        public string CardPath { get; set; }

        public Card(CardSuite suite, CardRank rank)
        {
            Suite = suite;
            Rank = rank;
        }

        public override bool Equals(object obj)
        {
            var otherCard = (Card)obj;
            return otherCard.Rank == this.Rank &&
                otherCard.Suite == this.Suite;
        }

        public override string ToString()
        {
            return IsHidden ? "back1" : Suite.ToString()[0].ToString() + Rank.GetRankString();
        }
    }
}
