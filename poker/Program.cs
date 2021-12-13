using System;
using System.Collections.Generic;
using System.Linq;

namespace poker
{
    class Program
    {
        static void Main(string[] args)
        {
            int bot_count = 2;
            List<Card> deck = new List<Card>();
            List<Card> user_deck = new List<Card>();
            foreach (Card.Suits suit in Enum.GetValues(typeof(Card.Suits)))
            {
                foreach (int rank in Enumerable.Range(1, 13))
                {
                    deck.Add(new Card(suit, rank));
                }
            }
            Console.WriteLine(deck.Count);
            Random random = new Random();
            foreach (int i in Enumerable.Range(1, 5))
            {
                int index = random.Next(0, deck.Count - 1);
                user_deck.Add(deck[index]);
            }
            foreach(Card card in user_deck)
            {
                Console.WriteLine(card.rank_value);
            }
        }
    }
}
