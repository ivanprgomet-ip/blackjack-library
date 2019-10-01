using BlackjackLibrary.Enums;
using BlackjackLibrary.Interfaces;

namespace BlackjackLibrary.Strategies.AfterClickBanking
{
    internal class DoubleDownActionPerformed
    {
        private IPlayer player;
        private IDealer dealer;

        public DoubleDownActionPerformed(IPlayer player, IDealer dealer)
        {
            this.player = player;
            this.dealer = dealer;
        }

        public void UpdateBanks(decimal initialPlayerBetAmount)
        {
            player.Balance -= initialPlayerBetAmount;
            dealer.Balance -= initialPlayerBetAmount;
            player.BetAmount += initialPlayerBetAmount;
            dealer.BetAmount += initialPlayerBetAmount;
        }
    }
}
