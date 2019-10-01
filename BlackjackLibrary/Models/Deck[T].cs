using BlackjackLibrary.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BlackjackLibrary.Models
{
    public class Deck<T>
    {
        public Stack<T> Cards { get; set; }
        public Deck()
        {
            Cards = new Stack<T>();
        }
        internal Deck<T> From(Stack<T> values)
        {
            return new Deck<T>()
            {
                Cards = values
            };
        }
        public static Deck<Card> CreateInstance()
        {
            return new Deck<Card>()
            {
                Cards = new Stack<Card>()
                .AddDeck()
                .AddDeck()
                .AddDeck()
                .AddDeck()
                .AddDeck()
                .AddDeck()
            };
        }

        public static Deck<Card> CreateInstance(bool shuffled)
        {
            var cards = shuffled ? new Stack<Card>().AddDeck().Shuffle()
                : new Stack<Card>().AddDeck();

            return new Deck<Card>() { Cards = cards };
        }

        internal static Deck<Card> CreateInstance(int decksCount)
        {
            var stacks = new Stack<Card>();
            foreach (var deck in GetStacks(decksCount))
                stacks.AddRange(deck);
            return new Deck<Card> { Cards = stacks };
        }

        private static IEnumerable<Stack<Card>> GetStacks(int decksCount)
        {
            for (int i = 0; i < decksCount; i++)
                yield return new Stack<Card>().AddDeck();
            yield break;
        }
    }
}