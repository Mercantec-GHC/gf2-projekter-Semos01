namespace Hjemmet
{
    public class TicTacToe
    {
        private char[] board = new char[9];
        private char player = 'X';
        private char ai = 'O';

        public void Start()
        {
            bool playAgain = true;
            while (playAgain)
            {
                InitBoard();
                player = 'X';
                Console.WriteLine("Velkommen til Kryds og Bolle!");
                PrintBoard();

                while (true)
                {
                    if (player == 'X')
                    {
                        PlayerMove();
                        if (IsGameOver(out char winner))
                        {
                            PrintBoard();
                            ShowResult(winner);
                            break;
                        }
                        player = ai;
                    }
                    else
                    {
                        AIMove();
                        if (IsGameOver(out char winner))
                        {
                            PrintBoard();
                            ShowResult(winner);
                            break;
                        }
                        player = 'X';
                    }
                    PrintBoard();
                }

                Console.WriteLine("Tryk på 0 for at vende tilbage til menuen eller på 1 for at afspille igen.");
                string? input = Console.ReadLine();
                if (input != "1")
                    playAgain = false;
            }
        }

        private void InitBoard()
        {
            for (int i = 0; i < 9; i++)
                board[i] = (char)('1' + i);
        }

        private void PrintBoard()
        {
            Console.WriteLine();
            for (int i = 0; i < 9; i += 3)
            {
                Console.WriteLine($" {board[i]} | {board[i + 1]} | {board[i + 2]} ");
                if (i < 6) Console.WriteLine("---+---+---");
            }
            Console.WriteLine();
        }

        private void PlayerMove()
        {
            int move;
            while (true)
            {
                Console.Write("Vælg et felt (1-9): ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out move) && move >= 1 && move <= 9 && board[move - 1] != 'X' && board[move - 1] != 'O')
                {
                    board[move - 1] = 'X';
                    break;
                }
                Console.WriteLine("Ugyldigt valg, prøv igen.");
            }
        }

        private void AIMove()
        {
            int bestScore = int.MinValue;
            int move = -1;
            for (int i = 0; i < 9; i++)
            {
                if (board[i] != 'X' && board[i] != 'O')
                {
                    char backup = board[i];
                    board[i] = ai;
                    int score = Minimax(false);
                    board[i] = backup;
                    if (score > bestScore)
                    {
                        bestScore = score;
                        move = i;
                    }
                }
            }
            board[move] = ai;

        }

        private int Minimax(bool isMaximizing)
        {
            if (IsWinner(ai)) return 1;
            if (IsWinner('X')) return -1;
            if (IsDraw()) return 0;

            int bestScore = isMaximizing ? int.MinValue : int.MaxValue;
            char current = isMaximizing ? ai : 'X';

            for (int i = 0; i < 9; i++)
            {
                if (board[i] != 'X' && board[i] != 'O')
                {
                    char backup = board[i];
                    board[i] = current;
                    int score = Minimax(!isMaximizing);
                    board[i] = backup;
                    if (isMaximizing)
                        bestScore = Math.Max(score, bestScore);
                    else
                        bestScore = Math.Min(score, bestScore);
                }
            }
            return bestScore;
        }

        private bool IsGameOver(out char winner)
        {
            if (IsWinner('X'))
            {
                winner = 'X';
                return true;
            }
            if (IsWinner(ai))
            {
                winner = ai;
                return true;
            }
            if (IsDraw())
            {
                winner = ' ';
                return true;
            }
            winner = ' ';
            return false;
        }

        private bool IsWinner(char c)
        {
            int[,] wins = {
                    {0,1,2},{3,4,5},{6,7,8},
                    {0,3,6},{1,4,7},{2,5,8},
                    {0,4,8},{2,4,6}
                };
            for (int i = 0; i < 8; i++)
            {
                if (board[wins[i, 0]] == c && board[wins[i, 1]] == c && board[wins[i, 2]] == c)
                    return true;
            }
            return false;
        }

        private bool IsDraw()
        {
            for (int i = 0; i < 9; i++)
                if (board[i] != 'X' && board[i] != 'O')
                    return false;
            return true;
        }

        public void ShowResult(char winner)
        {
            if (winner == 'X')
                Console.WriteLine("Tillykke! Du har vundet!");
            else if (winner == ai)
                Console.WriteLine("AI har vundet. Prøv igen!");
            else
                Console.WriteLine("Uafgjort!");
        }
    }
}