using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp3
{
    internal class ChessBoard
    {
        Grid grid;
        Tower tower;
        Pawn pawn;
        private readonly bool[,] _chessBoard = new bool[8, 8];

        public ChessBoard(Grid grid, Tower tower, Pawn pawn)
        {
            this.grid = grid;
            this.tower = tower;
            this.pawn = pawn;
            GridDefinitions();
        }
        private void Restart(object sender, RoutedEventArgs e)
        {
            tower.GeneratePosition();
            pawn.GeneratePosition();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    _chessBoard[i, j] = false;
                }
            }
            tower.TowerMoves(GetChessBoard(), pawn);
            DisplayGUI();
        }
        private void GridDefinitions()
        {
            for (int i = 0; i < 10; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
            }
            grid.RowDefinitions.Add(new RowDefinition());
        }



        public bool[,] GetChessBoard()
        {
            return _chessBoard;
        }

        public bool IsParity(int i, int j)
        {
            if ((i + j) % 2 == 0)
            {
                return true;
            }
            return false;
        }

        public void DisplayConsoleBool()
        {
            Console.WriteLine("**********************************************");
            for (var i = 0; i < _chessBoard.GetLength(0); i++)
            {
                Console.Write(i + "\t");
                for (var j = 0; j < _chessBoard.GetLength(1); j++)
                {
                    if (i == 0)
                    {
                        Console.Write(j + "\t");
                    }
                    else
                    {
                        Console.Write(_chessBoard[i, j] + "\t");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("**********************************************");
        }//Bool values board in console

        public void DisplayConsolePretty()
        {
            Console.WriteLine("**********************************************");
            Console.Write("  ");
            for (var i = 1; i < _chessBoard.GetLength(1) + 1; i++)
            {
                Console.Write(i);
            }
            Console.WriteLine();
            for (var i = 0; i < _chessBoard.GetLength(0); i++)
            {
                Console.Write((char)(i + 65) + " ");
                for (var j = 0; j < _chessBoard.GetLength(0); j++)
                {
                    if (i == tower.GetX() && j == tower.GetY())
                    {
                        Console.Write("\u2656");
                    }
                    else if (i == pawn.GetX() && j == pawn.GetY())
                    {
                        Console.Write("\u265F");
                    }
                    else if (_chessBoard[i, j])
                    {
                        Console.Write("\u25C6");
                    }
                    else if (j % 2 == 0)
                    {
                        Console.Write("\u2592");
                    }
                    else
                    {
                        Console.Write("\u2588");
                    }
                }

                Console.WriteLine();
            }
            Console.WriteLine("**********************************************");
        } //Pretty display in console

        public void DisplayGUI()
        {
            Button button = new Button()
            {
                Content = "Restart",
                Margin = new Thickness(0, 5, 0, 0),
            };
            Grid.SetColumn(button, 0);
            Grid.SetRow(button, 10);
            Grid.SetColumnSpan(button, 3);
            button.Click += Restart;
            grid.Children.Add(button);

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (j == 0 || j == 9 || i == 0 || i == 9)
                    {
                        for (int k = 1; k <= 8; k++)
                        {
                            Rectangle rectangle = new Rectangle()
                            {
                                Fill = Brushes.Black
                            };
                            Grid.SetColumn(rectangle, i);
                            Grid.SetRow(rectangle, j);
                            grid.Children.Add(rectangle);
                            Label label = new Label()
                            {
                                Content = i == 0 && j == 0 || i == 0 && j == 9 || i == 9 && j == 0 || i == 9 && j == 9 ? "" :
                                j == 0 || j == 9 ? $"{i}" : $"{(char)(j + 64)}",
                                Foreground = Brushes.White,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalContentAlignment = VerticalAlignment.Center,
                                FontSize = 35,
                            };
                            Grid.SetColumn(label, i);
                            Grid.SetRow(label, j);
                            grid.Children.Add(label);
                        }
                    }
                    else
                    {
                        Rectangle rectangle = new Rectangle()
                        {
                            Fill = pawn.IsCaptured(_chessBoard, i - 1, j - 1) ?
                        IsParity(i - 1, j - 1) ? Brushes.Red : Brushes.Salmon :
                        _chessBoard[i - 1, j - 1] ? IsParity(i - 1, j - 1) ? Brushes.Green : Brushes.LightGreen :
                        IsParity(i - 1, j - 1) ? Brushes.SaddleBrown : Brushes.BlanchedAlmond
                        };
                        Grid.SetColumn(rectangle, i);
                        Grid.SetRow(rectangle, j);
                        grid.Children.Add(rectangle);
                        if (i - 1 == tower.GetX() && j - 1 == tower.GetY())
                        {
                            Label label = new Label()
                            {
                                Content = "\u265C",
                                Foreground = Brushes.Brown,
                                FontSize = 35,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalContentAlignment = VerticalAlignment.Center
                            };
                            Grid.SetColumn(label, i);
                            Grid.SetRow(label, j);
                            grid.Children.Add(label);
                        }
                        else if (i - 1 == pawn.GetX() && j - 1 == pawn.GetY())
                        {
                            Label label = new Label()
                            {
                                Content = "\u265A",
                                Foreground = Brushes.SandyBrown,
                                FontSize = 35,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalContentAlignment = VerticalAlignment.Center
                            };
                            Grid.SetColumn(label, i);
                            Grid.SetRow(label, j);
                            grid.Children.Add(label);
                        }
                    }
                }
            }
        }
    }


    internal class Tower
    {
        private int _x;
        private int _y;

        public Tower()
        {
            GeneratePosition();
        }

        public void GeneratePosition()
        {
            var random = new Random();
            _x = random.Next(0, 8);
            _y = random.Next(0, 8);
        }

        public int GetX()
        {
            return _x;
        }

        public int GetY()
        {
            return _y;
        }

        public void TowerMoves(bool[,] board, Pawn pawn)
        {
            for (var i = 0; i < board.GetLength(0); i++)
                for (var j = 0; j < board.GetLength(1); j++)
                    board[i, j] = false;

            for (var i = _x - 1; i >= 0; i--)
            {
                board[i, _y] = true;
                if (i == pawn.GetX() && _y == pawn.GetY()) break;
            }

            for (var i = _x + 1; i < board.GetLength(0); i++)
            {
                board[i, _y] = true;
                if (i == pawn.GetX() && _y == pawn.GetY()) break;
            }

            for (var j = _y - 1; j >= 0; j--)
            {
                board[_x, j] = true;
                if (_x == pawn.GetX() && j == pawn.GetY()) break;
            }

            for (var j = _y + 1; j < board.GetLength(1); j++)
            {
                board[_x, j] = true;
                if (_x == pawn.GetX() && j == pawn.GetY()) break;
            }

            board[_x, _y] = true;
        }
    }


    internal class Pawn
    {
        private int _x;
        private int _y;

        public Pawn()
        {
            GeneratePosition();
        }

        public void GeneratePosition()
        {
            var random = new Random();
            _x = random.Next(0, 8);
            _y = random.Next(0, 8);
        }

        public int GetX()
        {
            return _x;
        }

        public int GetY()
        {
            return _y;
        }

        public bool IsCaptured(bool[,] chessBoard, int i, int j)
        {
            if (_x == i && _y == j && chessBoard[i, j])
            {
                return true;
            }
            return false;
        }
    }


    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Tower tower = new Tower();
            Pawn pawn = new Pawn();
            ChessBoard chessBoard = new ChessBoard(MainGrid, tower, pawn);
            tower.TowerMoves(chessBoard.GetChessBoard(), pawn);
            chessBoard.DisplayGUI();
        }
    }
}