using BlackjackLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackjackLibrary.Models
{
    internal static class DeckExtensions
    {
        public static Stack<Card> Shuffle(this Stack<Card> cards) {
            var rnd = new Random();
            var cardsArr = cards.ToArray();
            cards.Clear();
            foreach (var value in cardsArr.OrderBy(c => rnd.Next()))
                cards.Push(value);
            return cards;
        }

        public static Deck<T> Shuffle<T>(this Deck<T> deck) {
            var rnd = new Random();
            var cardsArr = deck.Cards.ToArray();
            deck.Cards.Clear();
            foreach (var value in cardsArr.OrderBy(c => rnd.Next()))
                deck.Cards.Push(value);
            return deck;
        }

        public static Stack<Card> AddDeck(this Stack<Card> cards) {
            for (int i = 0; i < Constants.Constants.CARDS_SUITES_COUNT; i++) {
                for (int j = 1; j <= Constants.Constants.CARDS_RANKS_COUNT; j++) {
                    switch (i)
                    {
                        case 0:
                            cards.Push(new Card(CardSuite.clubs, (CardRank)j));
                            break;
                        case 1:
                            cards.Push(new Card(CardSuite.diamonds, (CardRank)j));
                            break;
                        case 2:
                            cards.Push(new Card(CardSuite.hearts, (CardRank)j));
                            break;
                        case 3:
                            cards.Push(new Card(CardSuite.spades, (CardRank)j));
                            break;
                        default:
                            break;
                    }
                }
            }
            return cards;
        }

        public static T OneFromTop<T>(this Deck<T> deck) {
            return deck.Cards.Pop();
        }

        public static Stack<Card> AddRange(this Stack<Card> cards, Stack<Card> add)
        {
            foreach (var card in add)
                cards.Push(card);
            return cards;
        }

        public static decimal GetDecksLeft(this Deck<Card> deck)
        {
            return Math.Round(deck.Cards.Count/52M, 2);
        }
    }
}