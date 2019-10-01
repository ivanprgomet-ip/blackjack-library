using BlackjackLibrary.Enums;
using BlackjackLibrary.Interfaces;

namespace BlackjackLibrary.Strategies.RegularBanking
{
    internal class DealerWins : IBankingStrategy
    {
        public IPlayer player { get; }
        public IDealer dealer { get; }

        public DealerWins(IPlayer player, IDealer dealer)
        {
            this.player = player;
            this.dealer = dealer;
        }

        public void UpdateBank()
        {
            dealer.Balance += dealer.BetAmount;
            dealer.Balance += player.BetAmount;
        }
    }
}
