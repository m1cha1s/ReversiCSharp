using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reversi
{
    public partial class Form1 : Form
    {
        Board board = new Board();

        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            board.Draw(g, pictureBox1.Width, pictureBox1.Height);
            board.HighlightLegal(pictureBox1.Width, pictureBox1.Height, g);
            
            player1Score.Text = String.Format("Player 1: {0}", board.players[0].points);
            player2Score.Text = String.Format("Player 2: {0}", board.players[1].points);
            
            String turn = "<-";
            if (board.turn == 1)
                turn = "->";

            turnIndicator.Text = turn;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point mousePos = me.Location;

            if(board.IfLegal(mousePos.X, mousePos.Y, pictureBox1.Width, pictureBox1.Height))
                board.AddToken(mousePos.X, mousePos.Y, pictureBox1.Width, pictureBox1.Height);

            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            
        }
    }
}
