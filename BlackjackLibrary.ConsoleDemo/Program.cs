using BlackjackLibrary.API;
using BlackjackLibrary.EventArgs;
using BlackjackLibrary.Models;
using System;

namespace BlackjackLibrary.ConsoleDemo
{
    class Program
    {
        private static BlackjackGame game;
        static void Main(string[] args)
        {
            do
            {
                game = new BlackjackGame(new AiDealer("dealer", 1000), new HumanPlayer("player", 50), 6);
                game.GameOver += OnGameOver;
                game.RoundEvaluated += OnRoundEvaluated;
                game.DealerInfo.CardDealt += OnCardDealt;
                game.OnValidationError += OnValidationError;

                while (game.GameRunning)
                {
                    Console.Clear();
                    PrintHeader();
                    Console.WriteLine("how much do you wanna bet?");
                    game.StartRound(Console.ReadLine());

                    while (game.RoundRunning)
                    {
                        Console.Write("h/s/d >> ");
                        var choice = Console.ReadLine();

                        switch (choice)
                        {
                            case "h":
                                game.Hit();
                                break;
                            case "s":
                                game.Stand();
                                break;
                            case "d":
                                game.DoubleDown();
                                break;
                            default:
                                break;
                        }
                    }
                }
            } while (game.AppRunning);
        }

        private static void PrintHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(game.MainPlayer.Name + ": " + game.MainPlayer.Balance + "$ (" + game.MainPlayer.BetAmount + "$) "
                + game.DealerInfo.Name + ": " + game.DealerInfo.Balance + "$ (" + game.CurrentPot + "$) ");
            Console.ResetColor();
        }
        private static void OnValidationError(object sender, OnValidationErrorEventArgs e)
        {
            Console.WriteLine(e.validationMessage);
            Console.ReadLine();
        }
        private static void OnGameOver(object sender, GameOverEventArgs e)
        {
            Console.WriteLine(e.GameOverResultsString);
            Console.WriteLine("Do you wanna play again? (y/n)");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "y":
                    game.RestartGame();
                    break;
                case "n":
                    game.Exit();
                    break;
                default:
                    break;
            }
        }
        private static void OnCardDealt(object sender, CardDealtEventArgs e)
        {
            Console.Clear();

            PrintHeader();

            Console.Write(game.MainPlayer.Name + ":\t\t");
            foreach (var card in game.MainPlayer.Hand)
                Console.Write(card.ToString() + " ");
            Console.Write("<" + game.GetHandScore(game.MainPlayer.Hand) + "> " + game.MainPlayer.Status);
            Console.WriteLine();

            Console.Write(game.DealerInfo.Name + ":\t\t");
            foreach (var card in game.DealerInfo.Hand)
                Console.Write(card.ToString() + " ");
            Console.Write("<" + game.GetHandScore(game.DealerInfo.Hand) + "> " + game.DealerInfo.Status);
            Console.WriteLine();
        }
        private static void OnRoundEvaluated(object sender, RoundEvaluatedEventArgs e)
        {
            Console.WriteLine(e.Results.InfoText);
            Console.WriteLine("Press to continue");
            Console.ReadLine();
        }
    }
}
