using System;
namespace poker
{
    public class Card
    {
        public enum Suits
        {
            Diamonds = 0,
            Hearts = 1,
            Clubs = 2,
            Spades = 3
        }
        public Suits suit;
        public int rank;
        public String rank_value;
        private int value;
        private Suits val;

        public Card()
        {
            suit = Suits.Diamonds;
            rank = 0;
            rank_value = "";
        }
        public Card(Suits suit, int rank) {
            this.suit = suit;
            this.rank = rank;
            if (rank > 10) {
                if (rank == 11) {
                    this.rank_value = "jack";
                } else if (rank == 12) {
                    this.rank_value = "queen";
                } else if (rank == 13) {
                    this.rank_value = "king";
                } else if (rank == 14)
                {
                    this.rank_value = "A";
                }
            } else {
                this.rank_value = rank.ToString();
            }
        }
    }
}
