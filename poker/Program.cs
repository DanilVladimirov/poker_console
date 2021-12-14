using System;
using System.Collections.Generic;
using System.Linq;

namespace poker
{
    class Program
    {
        public static List<Card> deck = new List<Card>();
        public static int startMoney = 1000;
        public static int startDep = 100;
        public static int roundsCount = 3;
        public static int botMinimalDep = 50;

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
                foreach (int rank in Enumerable.Range(2, 14))
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

        public static void printPlayersMoney(Dictionary<String, int> playersMoney)
        {
            foreach (KeyValuePair<string, int> kvp in playersMoney)
            {
                Console.WriteLine("# " + kvp.Key + " - " + kvp.Value.ToString() + "$");
            }
        }

        public static bool isPlayerLose(int money)
        {
            if (money <= 0 && (money - startDep) <= 0)
            {
                return true;
            }
            return false;
        }

        static void Main(string[] args)
        {
            int bot_count = 2;
            Dictionary<String, int> playersMoney = new Dictionary<string, int>();
            playersMoney.Add("user", startMoney);

            foreach (int i in Enumerable.Range(0, bot_count))
            {
                playersMoney.Add("bot" + i.ToString(), startMoney);
            }

            foreach (int round in Enumerable.Range(0, bot_count))
            {
                int bank = 0;
                bool isSkeepDep = false;
                int last_dep = 0;
                Console.WriteLine("####### ROUND " + (round + 1).ToString() + " #######");
                // start deps

                if (isPlayerLose(playersMoney["user"]))
                {
                    Console.WriteLine("u lose :(");
                    break;
                } else
                {
                    playersMoney["user"] = playersMoney["user"] - startDep;
                    bank += startDep;
                }

                foreach(KeyValuePair<string, int> kvp in playersMoney)
                {
                    if (kvp.Key != "user")
                    {
                        if(isPlayerLose(kvp.Value))
                        {
                            Console.WriteLine("* player " + kvp.Key + " is loser *");
                            playersMoney.Remove(kvp.Key);
                        } else
                        {
                            playersMoney[kvp.Key] = playersMoney[kvp.Key] - startDep;
                            bank += startDep;
                        }
                    }
                }

                printPlayersMoney(playersMoney);

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
                    Console.WriteLine("throw card or end? n/y");
                    String choose = Console.ReadLine();
                    if (choose == "y")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("write index of card");
                        String input = Console.ReadLine();
                        user_deck.RemoveAt(Convert.ToInt32(input));
                    }

                }

                user_deck = fillDeck(user_deck);
                Console.WriteLine("Now your deck is");
                printDeck(user_deck);


                Console.WriteLine("Skip or dep? s/d");
                string chooseDep = Console.ReadLine();
                if (chooseDep == "s")
                {
                    isSkeepDep = true;
                } else
                {
                    Console.WriteLine("input your dep");
                    while (true)
                    {
                        int dep = Convert.ToInt32(Console.ReadLine());
                        if (dep < last_dep)
                        {
                            Console.WriteLine("Your dep is less then last");
                        }
                        if (dep > playersMoney["user"])
                        {
                            Console.WriteLine("Chill bro, u dont have this");
                        } else
                        {
                            last_dep = dep;
                            playersMoney["user"] = playersMoney["user"] - dep;
                            bank += dep;
                            break;
                        }
                    }
                }
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
                if (!isSkeepDep) {
                    int user_stat = checkDeck(user_deck);
                    Console.WriteLine("user stat:" + user_stat.ToString());
                    stats.Add("user", user_stat);
                } else
                {
                    Console.WriteLine("** u skip this round **");
                }

                foreach (int i in Enumerable.Range(0, bot_count))
                {
                    Console.WriteLine("bot - " + i.ToString() + " stat:" + checkDeck(bot_decks[i]).ToString());
                    stats.Add("bot" + i.ToString(), checkDeck(bot_decks[i]));
                }
                var sortedDict = from entry in stats orderby entry.Value descending select entry;
                foreach (KeyValuePair<string, int> kvp in sortedDict)
                {
                    Console.WriteLine(kvp.Value.ToString());
                }
                string winner = sortedDict.First().Key;
                Console.WriteLine("Winner of round is: " + sortedDict.First().Key);
                playersMoney[winner] += bank;
                bank = 0;
            }
            printPlayersMoney(playersMoney);
            var sortedMoney = from entry in playersMoney orderby entry.Value descending select entry;
            Console.WriteLine("$$$ Winner is: " + sortedMoney.First().Key + " $$$");
        }
    }
}
