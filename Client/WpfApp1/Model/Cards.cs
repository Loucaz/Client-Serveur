using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{

    public class Cards
    {
        public Cards()
        {
            Line = new List<Card>();
        }
        public int Index { get; set; }
        public List<Card> Line { get; set; }

        public static int getPoints(int value)
        {
            if (value == 55)
            {
                return 7;
            }
            else if (value % 11 == 0)
            {
                return 5;
            }
            else if (value % 10 == 0)
            {
                return 3;
            }
            else if (value % 5 == 0)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }
    }

    public class Card
    {
        public int Num { get; set; }

        public int Malus { get; set; }
    }

}
