using BlackjackLibrary.Enums;
using BlackjackLibrary.Interfaces;

namespace BlackjackLibrary.Strategies.RegularBanking
{
    internal class PlayerWinsNatural21 : IBankingStrategy
    {
        public IPlayer player { get; }
        public IDealer dealer { get; }

        public PlayerWinsNatural21(IPlayer player, IDealer dealer)
        {
            this.player = player;
            this.dealer = dealer;
        }

        public void UpdateBank()
        {
            var blackjackBonusAmount = (1.5m * player.BetAmount);
            player.Balance += blackjackBonusAmount;
            dealer.Balance -= (blackjackBonusAmount / 3);
        }
    }
}
