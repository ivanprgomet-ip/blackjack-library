using BlackjackLibrary.Enums;
using BlackjackLibrary.Interfaces;

namespace BlackjackLibrary.Strategies.RegularBanking
{
    internal class Draw : IBankingStrategy
    {
        public IPlayer player { get; }
        public IDealer dealer { get; }

        public Draw(IPlayer player, IDealer dealer)
        {
            this.player = player;
            this.dealer = dealer;
        }

        public void UpdateBank()
        {
            dealer.Balance += player.BetAmount;
            player.Balance += player.BetAmount;
        }
    }
}
