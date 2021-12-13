using System;
using System.Collections.Generic;
using System.Linq;

namespace poker
{
    public class Combination
    {
        public enum Combinations
        {
            FALSE = 0,
            ONE_CARD = 1,
            PAIR = 2,
            DOUBLE_PAIR = 3,
            SET_TRIPLE = 4,
            STREET = 5,
            FLESH = 6,
            FULL_HOUSE = 7,
            KARE = 8,
            STREET_FLESH = 9,
            FLESH_ROYALE = 10 // TODO
        }

        public Combination()
        {

        }

        public Combinations isPair(List<Card> deck)
        {
            int countOfPairs = 0;
            int countOfThree = 0;
            int countOfFour = 0;
            deck = deck.OrderBy(x => x.rank).ToList();
            foreach (Card card in deck.ToList())
            {
                int sumMatch = deck.Where(x => (int)x.rank == (int)card.rank).Count();
                if (sumMatch == 2) {
                    countOfPairs += 1;
                }
                if (sumMatch == 3)
                {
                    countOfThree += 1;
                }
                if (sumMatch == 4)
                {
                    countOfFour += 1;
                }
                deck.RemoveAll(x => (int)x.rank == (int)card.rank);
            }
            if (countOfThree == 1 && countOfPairs == 1)
            {
                return Combinations.FULL_HOUSE;
            }
            if (countOfFour == 1)
            {
                return Combinations.KARE;
            }
            if (countOfThree == 1)
            {
                return Combinations.SET_TRIPLE;
            }
            if (countOfPairs == 2)
            {
                return Combinations.DOUBLE_PAIR;
            }
            if (countOfPairs == 1)
            {
                return Combinations.PAIR;
            }
            if (countOfPairs == 0 && countOfThree == 0 && countOfFour == 0)
            {
                return Combinations.ONE_CARD;
            }
            return Combinations.FALSE;
        }

        public Combinations isStreet(List<Card> deck) {
            deck = deck.OrderBy(x => x.rank).ToList();         
            Combinations street = Combinations.STREET;
            int i = 0;
            foreach (Card card in deck)
            {
                if (i != 4 && deck[i + 1].rank != card.rank + 1) {
                    street = Combinations.FALSE;
                    break;
                }
                i++;
            }
            return street;
        }

        public Combinations isStreetFleshOrFlesh(List<Card> deck)
        {
            Card firstCard = deck[0];
            foreach (Card card in deck)
            {
                if (card.suit != firstCard.suit)
                {
                    return Combinations.FALSE;
                }
            }
            if (isStreet(deck) != Combinations.FALSE)
            {
                return Combinations.STREET_FLESH;
            } else {
                return Combinations.FLESH;
            }
        }

    }
}
