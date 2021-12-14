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

        public static List<Card> fillDeck(List<Card> deck)
        {
            if (deck.Count != 5)
            {Random rnd = new Random();
                foreach(int i in Enumerable.Range(1, 4 - deck.Count + 1))
                {
                    deck.Add(getRandomCard());
                }
            }
            return deck;
        }

        public static List<Card> throwCardsBot(List<Card> deck)
        {
            Random random = new Random();
            int delCardsCount = random.Next(0, 4);
            foreach(int i in Enumerable.Range(1, delCardsCount))
            {
                deck.RemoveAt(0);
            }
            return deck;
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

        static void printDeck(List<Card> deck)
        {
            foreach (Card card in deck)
            {
                Console.WriteLine(card.rank_value);
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

            Console.WriteLine("Your deck is");

            printDeck(user_deck);

            foreach (int i in Enumerable.Range(1, 5))
            {
                if (user_deck.Count() == 0)
                {
                    break;
                }
                Console.WriteLine("Your deck is");
                printDeck(user_deck);
                Console.WriteLine("throw card or end? y/n");
                String choose = Console.ReadLine();
                if(choose == "y")
                {
                    break;
                } else {
                    Console.WriteLine("write index of card");
                    String input = Console.ReadLine();
                    user_deck.RemoveAt(Convert.ToInt32(input));
                }
               
            }

            user_deck = fillDeck(user_deck);
            Console.WriteLine("Now your deck is");
            printDeck(user_deck);


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

            // bots throw random cards

            foreach (int i in Enumerable.Range(0, bot_count))
            {
                bot_decks[i] = throwCardsBot(bot_decks[i]);
                bot_decks[i] = fillDeck(bot_decks[i]);
            }

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
