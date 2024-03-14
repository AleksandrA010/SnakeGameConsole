using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace SnakeGameConsole
{
    internal class Program
    {
        private const int MapWidth = 30;
        private const int MapHeight = 21;

        private const int ScreenWidth = MapWidth * 2;
        private const int ScreenHeight = MapHeight * 2;

        private const ConsoleColor HeadColor = ConsoleColor.DarkBlue;
        private const ConsoleColor BodyColor = ConsoleColor.Cyan;
        private const ConsoleColor FoodColor = ConsoleColor.Red;
        private const ConsoleColor BorderColor = ConsoleColor.Gray;

        private const int FrameMs = 200;
        static void Main()
        {
            Title = "Snake";
            CursorVisible = false;
            SetWindowSize(ScreenWidth, ScreenHeight + 1);
            SetBufferSize(ScreenWidth, ScreenHeight + 1);

            while (true)
            {
                StarGame();
                Thread.Sleep(1000);
                ReadKey();
            }
        }

        static void StarGame()
        {
            Clear();

            DrawBorder();

            Direction currentMovement = Direction.Right;

            var snake = new Snake(10, 10, HeadColor, BodyColor);

            Pixel food = GenFood(snake);
            food.Draw();

            int score = 0;
            int lags = 0;

            Stopwatch sw = new Stopwatch();

            while (true)
            {
                sw.Restart();

                Direction oldMovement = currentMovement;

                while (sw.ElapsedMilliseconds <= FrameMs - lags)
                {
                    if (currentMovement == oldMovement)
                        currentMovement = ReadMovement(currentMovement);
                }

                sw.Restart();

                if (snake.Head.X == food.X && snake.Head.Y == food.Y)
                {
                    snake.Move(currentMovement, true);

                    food = GenFood(snake);
                    food.Draw();

                    score++;

                    Task.Run(() => Beep(1200, 200));
                }

                else
                    snake.Move(currentMovement, false);

                if (snake.Head.X == MapWidth - 1
                    || snake.Head.X == 0
                    || snake.Head.Y == MapHeight - 1
                    || snake.Head.Y == 1
                    || snake.Body.Any(b => b.X == snake.Head.X && b.Y == snake.Head.Y))
                    break;

                lags = (int)sw.ElapsedMilliseconds;
            }

            Clear();
            ForegroundColor = ConsoleColor.Cyan;
            string strEnd = $"Игра окончена! Ваш счёт — {score}";
            Task.Run(() => Beep(200, 600));
            SetCursorPosition((WindowWidth - strEnd.Length) / 2, WindowHeight / 2 - 1);
            Write(strEnd);
        }
        static Direction ReadMovement(Direction currentDirection)
        {
            if (!KeyAvailable)
                return currentDirection;

            ConsoleKey key = ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow when currentDirection != Direction.Down:
                    currentDirection = Direction.Up;
                    break;
                case ConsoleKey.DownArrow when currentDirection != Direction.Up:
                    currentDirection = Direction.Down;
                    break;
                case ConsoleKey.RightArrow when currentDirection != Direction.Left:
                    currentDirection = Direction.Right;
                    break;
                case ConsoleKey.LeftArrow when currentDirection != Direction.Right:
                    currentDirection = Direction.Left;
                    break;
            }

            return currentDirection;
        }
        static Pixel GenFood(Snake snake)
        {
            Pixel food;
            do
            {
                food = new Pixel(new Random().Next(1, MapWidth - 2), new Random().Next(2, MapHeight - 2), FoodColor);
            }
            while (snake.Head.X == food.X && snake.Head.Y == food.Y
            || snake.Body.Any(b => b.X == food.X && b.Y == food.Y));

            return food;
        }
        static void DrawBorder()
        {
            for (int i = 0; i < MapWidth; i++)
            {
                new Pixel(i, 1, BorderColor).Draw();
                new Pixel(i, MapHeight - 1, BorderColor).Draw();
            }

            for (int i = 1; i < MapHeight; i++)
            {
                new Pixel(0, i, BorderColor).Draw();
                new Pixel(MapWidth - 1, i, BorderColor).Draw();
            }
        }
    }
}
