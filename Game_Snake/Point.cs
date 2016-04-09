using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Game_Snake
{
    class Point
    {
        public int x { get; set; }
        public int y { get; set; }
        public Point(int a, int b)
        {
            x = a;
            y = b;
        }
    }

    class Snake
    {
        public enum Direction
        {
            Forward, Back, Down, Up, Finish
        }
        public static int score { get; set; }
        public List<Point> snake { get; set; }
        private Point catchMe { get; set; }
        private bool EqualToWidth = false;
        private bool EqualToHeight = false;
        private bool EqualToYZero = false;
        private bool EqualToXZero = false;
        public ConsoleKeyInfo info;
         public Direction direction { get; set; }
        public delegate void Movement();
        public event Movement MoveForward;
        public event Movement MoveDown;
        public event Movement MoveUp;
        public event Movement MoveBack;
        public event Movement MoveStartX;
        public event Movement MoveStartY;
        public event Movement MoveYZero;
        public event Movement MoveXZero;
        public event Movement RandomPosition;
        public Snake()
        {
            Console.SetWindowSize(100, 50);
            direction = Direction.Forward;
            catchMe = new Point(20,30);
            MoveForward = ChangeCoordinatesMoveForward;
            MoveDown = ChangeCoordinatesMoveDown;
            MoveUp = ChangeCoordinatesMoveUp;
            MoveBack = ChangeCoordinatesMoveBack;
            MoveStartX = ChangeCoordinatesMoveFomTheBeginingOfX;
            MoveStartY = ChangeCoordinatesMoveFomTheBeginingOfY;
            MoveYZero = ChangeCoordinatesMoveFomYZero;
            MoveXZero = ChangeCoordinatesMoveFomXZero;
            RandomPosition = GenerateRandomPosition;

            snake = new List<Point>();
            int x = 10, y = 10;
            for (int i = 0; i < 10; i++)
            {
                Point one = new Point(x-i, y);
                snake.Add(one);
            }
        }
        public void DrawSnake()
        {
            bool ok = true;
                ThreadStart TStart = new ThreadStart(GetCurrentDirection);
                Thread MyThread = new Thread(TStart);
                MyThread.Start();
            while (ok)
            {

                Console.Clear();

                for (int i = 0; i < 10; i++)
                {
                    if (snake[0].x == catchMe.x && snake[0].y == catchMe.y)
                        RandomPosition();
                    if (snake[i].x == Console.WindowWidth - 1)
                        EqualToWidth = true;
                    else if (snake[i].y == Console.WindowHeight - 1)
                        EqualToHeight = true;
                    else if (snake[i].y == 1)
                        EqualToYZero = true;
                    else if (snake[i].x == 1)
                        EqualToXZero = true;

                    Console.SetCursorPosition(snake[i].x, snake[i].y);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("*");
                }
                Console.SetCursorPosition(Console.WindowWidth - 9, 0);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("SCORE: " + score);
                Console.SetCursorPosition(catchMe.x, catchMe.y);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("$");
                switch (direction)
                {
                    case Direction.Forward:
                        {
                            if (EqualToWidth)
                                MoveStartX();
                            else
                                MoveForward();
                        }
                        break;
                    case Direction.Up:
                        {
                            if (EqualToYZero)
                                MoveYZero();
                            else
                                MoveUp();
                        }
                        break;
                    case Direction.Down:
                        {
                            if (EqualToHeight)
                                MoveStartY();
                            else
                                MoveDown();
                        }
                        break;
                    case Direction.Back:
                        {
                            if (EqualToXZero)
                                MoveXZero();
                            else
                                MoveBack();
                        }
                        break;
                    case Direction.Finish:
                        {
                            Console.Clear();
                            Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
                            Console.WriteLine("Bye!!!");
                            Console.SetCursorPosition(Console.WindowWidth / 2-5, Console.WindowHeight / 2-2);
                            Console.WriteLine("Your Score: " + score);
                            Thread.Sleep(300);
                        }
                        return;
                }

                Thread.Sleep(50);



                if (score == 20)
                {
                    ok = false;
                    Console.Clear();
                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
                    Console.WriteLine("You WON!!! Your Score: " + score);
                 
                }
            }
        }

        private void GetCurrentDirection()
        {
            while (true)
            {
                info = Console.ReadKey(true);
                if (info.Key == ConsoleKey.UpArrow)
                {
                    direction = Direction.Up;
                }
                else if (info.Key == ConsoleKey.RightArrow)
                {
                    direction = Direction.Forward;
                }
                else if (info.Key == ConsoleKey.LeftArrow)
                {
                    direction = Direction.Back;
                }
                else if (info.Key == ConsoleKey.DownArrow)
                {
                    direction = Direction.Down;
                }
                else if (info.Key == ConsoleKey.Escape)
                {
                    direction = Direction.Finish;
                }
            }
        }

        private void GenerateRandomPosition()
        {
            Random rnd = new Random();
            catchMe.x = rnd.Next(1, Console.WindowWidth-1);
            catchMe.y = rnd.Next(1, Console.WindowHeight - 1);
            score++;
        }

        private void ChangeCoordinatesMoveFomTheBeginingOfY()
        {
            for (int i = 0; i < 10; i++)
            {
                if (snake[i].y == (Console.WindowHeight - 1))
                    snake[i].y = 1;
                else
                    snake[i].y += 1;
            }
            EqualToHeight = false;
        }
        private void ChangeCoordinatesMoveFomTheBeginingOfX()
        {

            for (int i = 0; i < 10; i++)
            {
                if (snake[i].x == (Console.WindowWidth - 1))
                    snake[i].x = 1;
                else
                    snake[i].x += 1;

            }
            EqualToWidth = false;
        }
        private void ChangeCoordinatesMoveFomYZero()
        {
            for (int i = 9; i >= 0; i--)
            {
                if (snake[0].y == 1)
                {
                    snake[0].y = Console.WindowHeight - 1;
                    break;
                }
                else if (i == 0)
                {
                    snake[i].y -= 1;
                    break;
                }
                snake[i].x = snake[i - 1].x;
                snake[i].y = snake[i - 1].y;
            }
            EqualToYZero = false;
        }
        private void ChangeCoordinatesMoveFomXZero()
        {
            for (int i = 9; i >= 0; i--)
            {
                if (snake[0].x == 1)
                {
                    snake[0].x = Console.WindowWidth - 1;
                    break;
                }
                else if (i == 0)
                {
                    snake[i].x -= 1;
                    break;
                }
                snake[i].x = snake[i - 1].x;
                snake[i].y = snake[i - 1].y;
            }
            EqualToXZero = false;
        }
        private void ChangeCoordinatesMoveForward()
        {
            for (int i = 9; i >= 0; i--)
            {
                if (i == 0)
                {
                    snake[i].x += 1;
                    break;
                }
                snake[i].x = snake[i - 1].x;
                snake[i].y = snake[i - 1].y;
            }
        }
        private void ChangeCoordinatesMoveDown()
        {
            for (int i = 9; i >= 0; i--)
            {
                if (i == 0)
                {
                    snake[i].y += 1;
                    break;
                }
                snake[i].x = snake[i - 1].x;
                snake[i].y = snake[i - 1].y;
            }
        }
        private void ChangeCoordinatesMoveUp()
        {
            for (int i = 9; i >= 0; i--)
            {
                if (i == 0)
                {
                    snake[i].y -= 1;
                    break;
                }
                snake[i].x = snake[i - 1].x;
                snake[i].y = snake[i - 1].y;
            }
        }
        private void ChangeCoordinatesMoveBack()
        {
                for (int i = 9; i >= 0; i--)
                {
                    if (i == 0)
                    {
                        snake[i].x -= 1;
                        break;
                    }
                    snake[i].x = snake[i - 1].x;
                    snake[i].y = snake[i - 1].y;
                }
            }
     }
}
