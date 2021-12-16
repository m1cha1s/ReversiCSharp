using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Reversi
{
    class Player
    {
        public int ID;
        public int points = 0;

        public Player(int iD)
        {
            ID = iD;
        }

        public void AddPoints(int amount)
        {
            points += amount;
        }

        public void Draw(Graphics g)
        {

        }
    }
}
