using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Reversi
{
    class Board
    {
        public int turn = 0;

        public List<Player> players = new List<Player>();

        public List<List<int>> board = new List<List<int>>();

        public Board(int playerCount = 2, int boardSize = 8)
        {
            for (int i = 0; i < playerCount; i++)
            {
                players.Add(new Player(i));
            }

            for (int y = 0; y < boardSize; y++)
            {
                board.Add(new List<int>());
                for (int x = 0; x < boardSize; x++)
                {
                    board[y].Add(-1);
                }
            }

            board[boardSize / 2][boardSize / 2]           = 0;
            board[(boardSize / 2) - 1][boardSize / 2]     = 1;
            board[boardSize / 2][(boardSize / 2)-1]       = 1;
            board[(boardSize / 2) - 1][(boardSize / 2)-1] = 0;
        }

        public void Draw(Graphics g, int w, int h)
        {
            for(int y = 0; y < board.Count; y++)
            {
                for(int x = 0; x < board[y].Count; x++)
                {
                    if (board[y][x] >= 0)
                    {
                        Brush b = Brushes.White;

                        if (board[y][x] == 1)
                            b = Brushes.Black;

                        g.FillEllipse(b, (w / board[y].Count) * x, (h / board.Count) * y, (w / board[y].Count), (h / board.Count));
                    }
                }
            }
        }

        public int Dist(int x, int y)
        {
            return Math.Abs(x - y);
        }

        public void CheckTokens(int x, int y)
        {
            // down

            for (int ty = y+1; ty < board.Count; ty++)
            {
                if(board[ty][x] == board[y][x])
                {
                    for (int dy = y; dy < ty; dy++)
                    {
                        if (board[dy][x] != turn && board[dy][x] != -1)
                            players[turn].points++;
                        board[dy][x] = turn;
                    }
                    break;
                }
            }

            // up

            for (int ty = y-1; ty >= 0; ty--)
            {
                if (board[ty][x] == board[y][x])
                {
                    for (int dy = ty; dy < y; dy++)
                    {
                        if (board[dy][x] != turn && board[dy][x] != -1)
                            players[turn].points++;
                        board[dy][x] = turn;
                    }
                    break;
                }
            }

            // right

            for (int tx = x + 1; tx < board.Count; tx++)
            {
                if (board[y][tx] == board[y][x])
                {
                    for (int dx = x; dx < tx; dx++)
                    {
                        if (board[y][dx] != turn && board[y][dx] != -1)
                            players[turn].points++;
                        board[y][dx] = turn;
                    }
                    break;
                }
            }

            // left

            for (int tx = x - 1; tx >= 0; tx--)
            {
                if (board[y][tx] == board[y][x])
                {
                    for (int dx = tx; dx < x; dx++)
                    {
                        if (board[y][dx] != turn && board[y][dx] != -1)
                            players[turn].points++;
                        board[y][dx] = turn;
                    }
                    break;
                }
            }

            // down-right

            int tdy = y + 1;
            int tdx = x + 1;
            while(tdy < board.Count && tdx < board[0].Count)
            {
                if(board[tdy][tdx] == board[y][x])
                {
                    int ddy = y;
                    int ddx = x;
                    while(ddy < tdy && ddx < tdx)
                    {
                        if (board[ddy][ddx] != turn && board[ddy][ddx] != -1)
                            players[turn].points++;
                        board[ddy][ddx] = turn;
                        ddy++;
                        ddx++;
                    }
                    break;
                }
                tdy++;
                tdx++;
            }

            // up-left

            tdy = y - 1;
            tdx = x - 1;
            while (tdy >= 0 && tdx >= 0)
            {
                if (board[tdy][tdx] == board[y][x])
                {
                    int ddy = y;
                    int ddx = x;
                    while (ddy >= tdy && ddx >= tdx)
                    {
                        if (board[ddy][ddx] != turn && board[ddy][ddx] != -1)
                            players[turn].points++;
                        board[ddy][ddx] = turn;
                        ddy--;
                        ddx--;
                    }
                    break;
                }
                tdy--;
                tdx--;
            }
            
            // down-left

            tdy = y + 1;
            tdx = x - 1;
            while (tdy < board.Count && tdx >= 0)
            {
                if (board[tdy][tdx] == board[y][x])
                {
                    int ddy = y;
                    int ddx = x;
                    while (ddy < tdy && ddx >= tdx)
                    {
                        if (board[ddy][ddx] != turn && board[ddy][ddx] != -1)
                            players[turn].points++;
                        board[ddy][ddx] = turn;
                        ddy++;
                        ddx--;
                    }
                    break;
                }
                tdy++;
                tdx--;
            }
            
            // up-right

            tdy = y - 1;
            tdx = x + 1;
            while (tdy >= 0 && tdx < board[0].Count)
            {
                if (board[tdy][tdx] == board[y][x])
                {
                    int ddy = y;
                    int ddx = x;
                    while (ddy >= tdy && ddx < tdx)
                    {
                        if (board[ddy][ddx] != turn && board[ddy][ddx] != -1)
                            players[turn].points++;
                        board[ddy][ddx] = turn;
                        ddy--;
                        ddx++;
                    }
                    break;
                }
                tdy--;
                tdx++;
            }
        }

        public void CompleteTurn()
        {
            turn++;
            if(turn >= players.Count) {
                turn = 0;
            }
            bool possible = false;
            for (int y = 0; y < board.Count; y++)
            {
                for (int x = 0; x < board[y].Count; x++)
                {
                    possible = possible || IfLegal(x, y, board[y].Count, board.Count);
                }
            }
            if (!possible)
            {
                turn++;
                if (turn >= players.Count)
                {
                    turn = 0;
                }
            }
        }

        public bool Nerby(int x, int y)
        {
            if (board[y][x] != -1)
                return false;

            if (x > 0)
                if (board[y][x - 1] != -1)
                    return true;

            if (x < board[y].Count-1)
                if (board[y][x + 1] != -1)
                    return true;

            if (y > 0)
                if (board[y - 1][x] != -1)
                    return true;

            if (y < board.Count-1)
                if (board[y + 1][x] != -1)
                    return true;

            return false;
        }

        public bool IfLegal(int mx, int my, int w, int h)
        {
            int x = Convert.ToInt32(MathF.Floor(mx / (w / board[0].Count)));
            int y = Convert.ToInt32(MathF.Floor(my / (h / board.Count)));

            bool nerby = Nerby(x, y);

            // down

            for (int ty = y + 1; ty < board.Count; ty++)
            {
                if (board[ty][x] == board[y][x] && nerby)
                {
                    bool valid = true;
                    for (int dy = y; dy < ty; dy++)
                    {
                        if (board[dy][x] == turn || board[dy][x] == -1)
                        {
                            valid = false;
                            break;
                        }
                    }
                    if (valid)
                        return true;
                    else
                        break;
                }
            }

            // up

            for (int ty = y - 1; ty >= 0; ty--)
            {
                if (board[ty][x] == board[y][x] && nerby)
                {
                    bool valid = true;
                    for (int dy = ty; dy < y; dy++)
                    {
                        if (board[dy][x] == turn || board[dy][x] == -1)
                        {
                            valid = false;
                            break;
                        }
                    }
                    if (valid)
                        return true;
                    else
                        break;
                }
            }

            // right

            for (int tx = x + 1; tx < board.Count; tx++)
            {
                if (board[y][tx] == board[y][x] && nerby)
                {
                    bool valid = true;
                    for (int dx = x; dx < tx; dx++)
                    {
                        if (board[y][dx] == turn || board[y][dx] == -1)
                        {
                            valid = false;
                            break;
                        }
                    }
                    if (valid)
                        return true;else
                        break;
                }
            }

            // left

            for (int tx = x - 1; tx >= 0; tx--)
            {
                if (board[y][tx] == board[y][x] && nerby)
                {
                    bool valid = true;
                    for (int dx = tx; dx < x; dx++)
                    {
                        if (board[y][dx] == turn || board[y][dx] == -1)
                        {
                            valid = false;
                            break;
                        }
                    }
                    if (valid)
                        return true;
                    else
                        break;
                }
            }

            // down-right

            int tdy = y + 1;
            int tdx = x + 1;
            while (tdy < board.Count && tdx < board[0].Count)
            {
                if (board[tdy][tdx] == board[y][x] && nerby)
                {
                    bool valid = true;
                    int ddy = y;
                    int ddx = x;
                    while (ddy < tdy && ddx < tdx)
                    {
                        if (board[ddy][ddx] == turn || board[ddy][ddx] == -1)
                        {
                            valid = false;
                            break;
                        }
                        ddy++;
                        ddx++;
                    }
                    if(valid)
                        return true;
                    else
                        break;
                }
                tdy++;
                tdx++;
            }

            // up-left

            tdy = y - 1;
            tdx = x - 1;
            while (tdy >= 0 && tdx >= 0)
            {
                if (board[tdy][tdx] == board[y][x] && nerby)
                {
                    bool valid = true;
                    int ddy = y;
                    int ddx = x;
                    while (ddy >= tdy && ddx >= tdx)
                    {
                        if (board[ddy][ddx] != turn && board[ddy][ddx] != -1)
                        {
                            valid = false;
                            break;
                        }
                        ddy--;
                        ddx--;
                    }
                    if (valid)
                        return true;
                    else
                        break;
                }
                tdy--;
                tdx--;
            }

            // down-left

            tdy = y + 1;
            tdx = x - 1;
            while (tdy < board.Count && tdx >= 0)
            {
                if (board[tdy][tdx] == board[y][x] && nerby)
                {
                    bool valid = true;
                    int ddy = y;
                    int ddx = x;
                    while (ddy < tdy && ddx >= tdx)
                    {
                        if (board[ddy][ddx] != turn && board[ddy][ddx] != -1)
                        {
                            valid = false;
                            break;
                        }
                        ddy++;
                        ddx--;
                    }
                    if (valid)
                        return true;
                    else
                        break;
                }
                tdy++;
                tdx--;
            }

            // up-right

            tdy = y - 1;
            tdx = x + 1;
            while (tdy >= 0 && tdx < board[0].Count)
            {
                if (board[tdy][tdx] == board[y][x] && nerby)
                {
                    bool valid = true;
                    int ddy = y;
                    int ddx = x;
                    while (ddy >= tdy && ddx < tdx)
                    {
                        if (board[ddy][ddx] != turn && board[ddy][ddx] != -1)
                        {
                            valid = false;
                            break;
                        }
                        ddy--;
                        ddx++;
                    }
                    if (valid)
                        return true;
                    else
                        break;
                }
                tdy--;
                tdx++;
            }

            return false; 
        }

        public void HighlightLegal(int w, int h, Graphics g)
        {
            for (int y = 0; y < board.Count; y++)
            {
                for (int x = 0; x < board[y].Count; x++)
                {
                    if (IfLegal(x, y, board[0].Count, board.Count))
                    {
                        g.FillEllipse(Brushes.Gray, x * (w / board[0].Count), y * (h / board.Count), (w / board[y].Count), (h / board.Count));
                    }
                }
            }
            
        }

        public bool InBounds(int tx, int ty, int w, int h)
        {
            int x = Convert.ToInt32(MathF.Floor(tx / (w / board[0].Count)));
            int y = Convert.ToInt32(MathF.Floor(ty / (h / board.Count)));

            return x >= 0 && x < board[0].Count && y >= 0 && y < board.Count;
        }

        public void AddToken(int tx, int ty, int w, int h)
        {
            int x = Convert.ToInt32(MathF.Floor(tx / (w / board[0].Count))); 
            int y = Convert.ToInt32(MathF.Floor(ty / (h / board.Count)));

            if (board[y][x] == -1)
            {
                board[y][x] = turn;

                CheckTokens(x, y);

                CompleteTurn();
            }
        }
    }
}
