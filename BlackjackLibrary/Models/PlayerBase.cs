using BlackjackLibrary.Interfaces;
using BlackjackLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLibrary.Models
{
    public abstract class PlayerBase : IPlayer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public decimal BetAmount { get; set; }
        public HandStatus Status { get; set; }
        public List<Card> Hand { get; set; }

        public PlayerBase(string name, decimal balance)
        {
            Name = name;
            Balance = balance;
            Hand = new List<Card>();
            Id = Guid.NewGuid().ToString();
        }

        public abstract BlackjackDecision MakeMove(Func<List<Card>, int> calcHandValue);
    }
}
