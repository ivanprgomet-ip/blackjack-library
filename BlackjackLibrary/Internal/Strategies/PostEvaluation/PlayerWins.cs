using BlackjackLibrary.Enums;
using BlackjackLibrary.Interfaces;
using BlackjackLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackLibrary.Strategies.RegularBanking
{
    internal class PlayerWins : IBankingStrategy
    {
        public IPlayer player { get; }
        public IDealer dealer { get; }

        public PlayerWins(IPlayer player, IDealer dealer)
        {
            this.player = player;
            this.dealer = dealer;
        }

        public void UpdateBank()
        {
            player.Balance += player.BetAmount;
            player.Balance += dealer.BetAmount;
        }
    }
}
