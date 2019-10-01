using System;
using System.Collections.Generic;
using System.Linq;
using BlackjackLibrary.Enums;
using BlackjackLibrary.Interfaces;
using BlackjackLibrary.Models;
using BlackjackLibrary.Extensions;
using BlackjackLibrary.EventArgs;
using BlackjackLibrary.Strategies.AfterClickBanking;
using BlackjackLibrary.Strategies.RegularBanking;
using System.Reflection;

namespace BlackjackLibrary.API
{
    public partial class BlackjackGame
    {
        /// <summary>
        /// Fires whenever a round has finished
        /// </summary>
        public event EventHandler<RoundEvaluatedEventArgs> RoundEvaluated;
        /// <summary>
        /// Fires whenever the game is over due to player running out of money etc.
        /// </summary>
        public event EventHandler<GameOverEventArgs> GameOver;
        /// <summary>
        /// Occurs whenever a validation occurs and there is some type of insufficiency/error
        /// </summary>
        public event EventHandler<OnValidationErrorEventArgs> OnValidationError;

        private IBlackjackRegulations _regulations;
        private Deck<Card> _deck;
        private IPlayer _mainPlayer;
        private AiDealer _dealer;
        private IDealer _dealerInfo;

        private int DecksUsed { get; set; }
        /// <summary>
        /// Returns true for a round that is currently in progress, false for a round not currently running
        /// </summary>
        public bool RoundRunning { get; set; }
        /// <summary>
        /// Returns true if a game is currently running (multiple rounds), false for a game not running
        /// </summary>
        public bool GameRunning { get; set; }
        /// <summary>
        /// Returns true for application running
        /// </summary>
        public bool AppRunning { get; set; }
        /// <summary>
        /// Returns how much is currently in the pot
        /// </summary>
        public decimal CurrentPot { get; set; }

        private AiDealer Dealer
        {
            get
            {
                if (_dealer == null)
                    throw new ArgumentException("dealer is null");
                return _dealer;
            }
            set { _dealer = value; }
        }
        private IBlackjackRegulations Regulations
        {
            get
            {
                if (_regulations == null)
                    _regulations = new BlackjackRegulations();
                return _regulations;
            }
            set { _regulations = value; }
        }
        /// <summary>
        /// Returns the player object
        /// </summary>
        public IPlayer MainPlayer
        {
            get
            {
                if (_mainPlayer.IsNull())
                    throw new ArgumentException("Mainplayer is null");
                return _mainPlayer;
            }
            set { _mainPlayer = value; }
        }
        /// <summary>
        /// Returns how much of the deck(s) is currently dealt out, and how much is left
        /// </summary>
        public decimal DecksLeft
        {
            get
            {
                return Deck.GetDecksLeft();
            }
        }
        /// <summary>
        /// Returns the dealerinfo object containinig data for the dealer, and the OnCardDealt event
        /// </summary>
        public IDealer DealerInfo
        {
            get
            {
                if (_dealerInfo == null)
                    throw new ArgumentException("dealerinfo is null");
                return _dealerInfo;
            }
            set { _dealerInfo = value; }
        }
        private Deck<Card> Deck
        {
            get
            {
                if (_deck == null)
                    _deck = Deck<Card>.CreateInstance(DecksUsed).Shuffle();
                return _deck;
            }
            set { _deck = value; }
        }

        public BlackjackGame(AiDealer dealer, HumanPlayer player, int decksUsed)
        {
            Dealer = dealer.GuardNotNull();
            DealerInfo = dealer.GuardNotNull();
            MainPlayer = player.GuardNotNull();
            DecksUsed = decksUsed;

            if (player.Balance > dealer.Balance)
                throw new Exception("Dealer balance must be higher than player balance");
            else
            {
                GameRunning = true;
                AppRunning = true;
            }

        }

        /// <summary>
        /// Works in conjunction with AppRunning and GameRunning properties
        /// </summary>
        public void Exit()
        {
            AppRunning = false;
            GameRunning = false;
        }
        /// <summary>
        /// Works in conjunction with AppRunning and GameRunning properties
        /// </summary>
        public void RestartGame()
        {
            AppRunning = true;
            GameRunning = false;
        }
        private void ResetRound()
        {
            ResetPlayer(MainPlayer);
            ResetPlayer(Dealer);
            if (DecksLeft < Constants.Constants.MIN_DECKS_AT_PLAY_BEFORE_DECK_RESET)
                ResetDeck(6);
        }
        private void ResetDeck(int newDecksCount)
        {
            Deck = Deck<Card>.CreateInstance(newDecksCount);
        }
        private void EvaluateRound()
        {
            var winner = MainPlayer.Status == HandStatus.Natural
                ? Winninghand.PlayerOnNatural21
                : Regulations.EvaluateWinner(MainPlayer.Hand, Dealer.Hand);

            var results = GetRoundResults(MainPlayer, winner);
            UpdateBalancesAfterEvaluation(Dealer, MainPlayer as HumanPlayer, winner);
            RoundEvaluated?.Invoke(this, new RoundEvaluatedEventArgs(results));
            RoundRunning = false;

            if (MainPlayer.Balance <= 0)
            {
                GameOver?.Invoke(this, new GameOverEventArgs(MainPlayer.Name + " ran out of money. You Lose!"));
                GameRunning = false;
            }

            if (Dealer.Balance < MainPlayer.Balance)
            {
                GameOver?.Invoke(this, new GameOverEventArgs("Dealer doesn't have enough money to continue. You Win!"));
                GameRunning = false;
            }
        }
        private BlackjackDecision MakeTurnUpdateStatus(Decision decision)
        {
            var player = default(IPlayer);
            if (Dealer.Id == decision.PlayerId)
                player = Dealer;
            else
                player = MainPlayer;

            var dealCount = 1;
            switch (decision.PlayerDecision)
            {
                case BlackjackDecision.unspecified:
                    return decision.PlayerDecision;
                case BlackjackDecision.hit:
                    Dealer.DealTo(player, ref dealCount, Deck, Regulations.GetHandStatus);
                    return decision.PlayerDecision;
                case BlackjackDecision.stand:
                    return decision.PlayerDecision;
                default:
                    return decision.PlayerDecision;
            }
        }
        /// <summary>
        /// Adds another card to player hand
        /// </summary>
        public void Hit()
        {
            var dealCount = 1;
            Dealer.DealTo(MainPlayer, ref dealCount, Deck, Regulations.GetHandStatus);

            if (MainPlayer.Status == HandStatus.blackjack)
            {
                DealerTurn();
            }
            if (MainPlayer.Status == HandStatus.bust)
            {
                RevealHiddenCard();
                EvaluateRound();
            }
        }
        /// <summary>
        /// Dealers turn after player stands
        /// </summary>
        public void Stand()
        {
            DealerTurn();
        }
        /// <summary>
        /// Input valid player bet amount to start the initial deal
        /// </summary>
        /// <param name="betAmount"></param>
        public void StartRound(string betAmount)
        {
            bool betValid = decimal.TryParse(betAmount, out decimal validatedBetAmount);
            if (!string.IsNullOrEmpty(betAmount) && betValid)
            {
                if (MainPlayer.Balance >= validatedBetAmount && validatedBetAmount > 0)
                {
                    ResetRound();
                    RoundRunning = true;
                    MakeBet(validatedBetAmount);
                    InitialDeal();
                }
                else
                    OnValidationError?.Invoke(this, new OnValidationErrorEventArgs("Initial bet insufficient balance"));
            }
            else
                OnValidationError?.Invoke(this, new OnValidationErrorEventArgs("Initial bet not valid"));
        }
        private void DealerTurn()
        {
            RevealHiddenCard();
            DealerMove();
            EvaluateRound();
        }
        private void InitialDeal()
        {
            DealTo(MainPlayer, 2);
            DealTo(Dealer, 1, true);
            DealTo(Dealer, 1);

            if (MainPlayer.Status == HandStatus.Natural)
            {
                DealerTurn();
            }
        }
        /// <summary>
        /// Player gets another card and doubles betamount if possible
        /// </summary>
        public void DoubleDown()
        {
            if (MainPlayer.Balance >= MainPlayer.BetAmount)
            {
                UpdateBalancesAfterAction(MainPlayer, Dealer, MainPlayer.BetAmount, RoundType.DoubleDownClick);
                var count = 1;
                Dealer.DealTo(MainPlayer, ref count, Deck, Regulations.GetHandStatus);
                DealerTurn();
            }
            else
                throw new Exception("Insufficient balance for doubling");
        }
        private void MakeBet(decimal pBet)
        {
            UpdateBalancesAfterAction(MainPlayer, Dealer, pBet, RoundType.InitialBetClick);
        }

        private void UpdateBalancesAfterEvaluation(AiDealer dealer, HumanPlayer player, Winninghand winner)
        {
            switch (winner)
            {
                case Winninghand.Dealer:
                    new DealerWins(player, dealer).UpdateBank();
                    break;
                case Winninghand.Player:
                case Winninghand.PlayerOnFake21:
                    new PlayerWins(player, dealer).UpdateBank();
                    break;
                case Winninghand.Draw:
                case Winninghand.BothBust:
                    new Draw(player, dealer).UpdateBank();
                    break;
                case Winninghand.PlayerOnNatural21:
                    new PlayerWinsNatural21(player, dealer).UpdateBank();
                    break;
                default:
                    break;
            }

            ResetBetsAndCurrentpot(dealer, player,
                (d, p) =>
                {
                    p.BetAmount = 0;
                    d.BetAmount = 0;
                });
        }
        private void ResetBetsAndCurrentpot(AiDealer dealer, HumanPlayer player, params Action<IDealer, IPlayer>[] resetMethods)
        {
            for (int i = 0; i < resetMethods.Count(); i++)
                resetMethods[i](dealer, player);
            CurrentPot = 0;
        }
        private void UpdateBalancesAfterAction(IPlayer player, IDealer dealer, decimal initialPlayerBetAmount, RoundType roundType = RoundType.InitialBetClick)
        {
            switch (roundType)
            {
                case RoundType.unspecified:
                    break;
                case RoundType.InitialBetClick:
                    new InitialBetActionPerformed(player, dealer).UpdateBanks(initialPlayerBetAmount);
                    CurrentPot = sumBets(player.BetAmount, dealer.BetAmount);
                    break;
                case RoundType.DoubleDownClick:
                    new DoubleDownActionPerformed(player, dealer).UpdateBanks(initialPlayerBetAmount);
                    CurrentPot = sumBets(player.BetAmount, dealer.BetAmount);
                    break;
                default:
                    break;
            }
        }
        private decimal sumBets(params decimal[] bets)
        {
            return bets.Aggregate((a, b) => a + b);
        }
        private PlayerResultsData GetRoundResults(IPlayer player, Winninghand winner)
        {
            switch (winner)
            {
                case Winninghand.Dealer:
                    return new PlayerResultsData()
                    {
                        CurrentWinner = winner,
                        InfoText = string.Format($"{player.Name} lost {player.BetAmount}$"),
                        Amount = player.BetAmount,
                    };
                case Winninghand.Player:
                    return new PlayerResultsData()
                    {
                        CurrentWinner = winner,
                        InfoText = string.Format($"{player.Name} won {player.BetAmount}$"),
                        Amount = player.BetAmount * 2,
                    };
                case Winninghand.PlayerOnNatural21:
                    return new PlayerResultsData()
                    {
                        CurrentWinner = winner,
                        InfoText = string.Format($"{player.Name} won with a natural blackjack!"),
                        Amount = 1.5m * player.BetAmount,
                    };
                case Winninghand.Draw:
                    return new PlayerResultsData()
                    {
                        CurrentWinner = winner,
                        InfoText = string.Format($"{player.Name} draws"),
                        Amount = player.BetAmount,
                    };
                default:
                    return new PlayerResultsData();
            }
        }
        private Decision GetDealerDecision()
        {
            return new Decision(Dealer.Id, Dealer.ComputeDecision(Dealer.Hand.GetBlackjackHandScore()));
        }
        /// <summary>
        /// Computes the handvalue
        /// </summary>
        /// <param name="hand"></param>
        /// <returns>value of the hand</returns>
        public int GetHandScore(List<Card> hand) => hand.GetBlackjackHandScore();
        private void DealerMove()
        {
            var breakStatuses = new[] { HandStatus.bust, HandStatus.blackjack };
            var whileDecisions = new[] { BlackjackDecision.hit, BlackjackDecision.unspecified };

            BlackjackDecision dDecision;
            do
            {
                var decision = GetDealerDecision();
                dDecision = MakeTurnUpdateStatus(decision);
                if (breakStatuses.Any(s => s.Equals(Dealer.Status)))
                    break;
            } while (whileDecisions.Any(d => d.Equals(dDecision)));
        }
        private void RevealHiddenCard()
        {
            if (Dealer.Hand.Any(c => c.IsHidden))
            {
                var hiddenCard = Dealer.Hand.Where(c => c.IsHidden).FirstOrDefault();
                hiddenCard.IsHidden = false;
            }
        }
        private void DealTo(IPlayer player, int count) => Dealer.DealTo(player, ref count, Deck, Regulations.GetHandStatus);
        private void DealTo(IPlayer player, int count, bool facedown) => Dealer.DealTo(player, ref count, Deck, facedown, Regulations.GetHandStatus);
        private void ResetPlayer(IPlayer player)
        {
            player.BetAmount = 0;
            player.Hand.Clear();
            player.Status = HandStatus.unspecified;
        }
        /// <summary>
        /// Returns the current blackjack library assembly version
        /// </summary>
        /// <returns>blackjack library current version number</returns>
        public string GetLibraryVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
