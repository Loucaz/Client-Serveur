using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{

    class Player
    {
        public string name;
        public int points;

        public Player(string v1, string v2)
        {
            this.name = v1;
            int.TryParse(v2,out this.points);
        }
    }
}
