using BlackjackLibrary.Enums;
using BlackjackLibrary.Interfaces;

namespace BlackjackLibrary.Strategies.AfterClickBanking
{
    internal class InitialBetActionPerformed
    {
        private IPlayer player;
        private IDealer dealer;

        public InitialBetActionPerformed(IPlayer player, IDealer dealer)
        {
            this.player = player;
            this.dealer = dealer;
        }

        internal void UpdateBanks(decimal initialPlayerBetAmount)
        {
            player.Balance -= initialPlayerBetAmount;
            dealer.Balance -= initialPlayerBetAmount;
            player.BetAmount += initialPlayerBetAmount;
            dealer.BetAmount += initialPlayerBetAmount;
        }
    }
}
