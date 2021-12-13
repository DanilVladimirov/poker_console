using System;
using System.Collections.Generic;
using System.Linq;

namespace poker
{
    class Program
    {
        public static List<Card> deck = new List<Card>();

        static Card getRandomCard()
        {
            Random random = new Random();
            int index = random.Next(0, deck.Count - 1);
            return deck[index];
        }

        static void initDeck()
        {
            foreach (Card.Suits suit in Enum.GetValues(typeof(Card.Suits)))
            {
                foreach (int rank in Enumerable.Range(1, 13))
                {
                    deck.Add(new Card(suit, rank));
                }
            }
        }

        static int checkDeck(List<Card> deck)
        {
            Combination combinationManager = new Combination();
            Combination.Combinations street = combinationManager.isStreet(deck);
            if (street != Combination.Combinations.FALSE)
            {
                return (int) street;
            }
            Combination.Combinations pair = combinationManager.isPair(deck);
            if (pair != Combination.Combinations.FALSE)
            {
                return (int) pair;
            }
            Combination.Combinations flesh = combinationManager.isStreetFleshOrFlesh(deck);
            if (flesh != Combination.Combinations.FALSE)
            {
                return (int) flesh;
            }
            return (int) Combination.Combinations.FALSE;
        }

        static void Main(string[] args)
        {
            int bot_count = 2;
            
            List<Card> user_deck = new List<Card>();
            List<List<Card>> bot_decks = new List<List<Card>>();
            Random random = new Random();

            initDeck();

            foreach (int i in Enumerable.Range(1, 5))
            {
                user_deck.Add(getRandomCard());
            }

            //foreach(Card card in user_deck)
            //{
            //    Console.WriteLine(card.rank_value);
            //}

            // give cards to bots

            foreach (int i in Enumerable.Range(1, bot_count))
            {
                List<Card> bot_deck = new List<Card>();
                foreach (int j in Enumerable.Range(1, 5))
                {
                    bot_deck.Add(getRandomCard());
                }
                bot_decks.Add(bot_deck);
            }

            //foreach (int i in Enumerable.Range(0, bot_count))
            //{
            //    Console.WriteLine("bot - " + i.ToString());
            //    foreach (Card card in bot_decks[i])
            //    {

            //        Console.WriteLine(card.rank_value);
            //    }
            //}

            Dictionary<String, int> stats = new Dictionary<string, int>();
            int user_stat = checkDeck(user_deck);
            Console.WriteLine("user stat:" + user_stat.ToString());

            stats.Add("user", user_stat);
            foreach(int i in Enumerable.Range(0, bot_count))
            {
                Console.WriteLine("bot - " +i.ToString() +" stat:" + checkDeck(bot_decks[i]).ToString());
                stats.Add("bot"+i.ToString(), checkDeck(bot_decks[i]));
            }
            var sortedDict = from entry in stats orderby entry.Value descending select entry;
            foreach (KeyValuePair<string, int> kvp in sortedDict)
            {
                Console.WriteLine(kvp.Value.ToString());
            }
            Console.WriteLine("Winner is: " + sortedDict.First().Key);
        }
    }
}
