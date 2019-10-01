using System;
using System.Collections.Generic;
using BlackjackLibrary.Interfaces;
using BlackjackLibrary.Enums;

namespace BlackjackLibrary.Models
{
    public class HumanPlayer : PlayerBase
    {
        public HumanPlayer(string name, decimal balance) : base(name, balance) { }

        public override BlackjackDecision MakeMove(Func<List<Card>, int> calcHandValue) {
            throw new NotImplementedException();
        }
    }
}