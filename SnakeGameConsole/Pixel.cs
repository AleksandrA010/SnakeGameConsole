using System;

namespace SnakeGameConsole
{
    internal class Pixel
    {
        private const char PizelChar = '█';
        public int X { get; }
        public int Y { get; }
        public int PixelSize { get; }
        public ConsoleColor Color { get; }
        public Pixel(int x, int y, ConsoleColor color, int pixelSize = 2)
        {
            X = x;
            Y = y;
            Color = color;
            PixelSize = pixelSize;
        }
        public void Draw()
        {
            Console.ForegroundColor = Color;

            for (int x = 0; x < PixelSize; x++)
            {
                for (int y = 0; y < PixelSize; y++)
                {
                    Console.SetCursorPosition(X * PixelSize + x, Y * PixelSize + y);
                    Console.Write(PizelChar);
                }
            }
        }
        public void Clear()
        {
            for (int x = 0; x < PixelSize; x++)
            {
                for (int y = 0; y < PixelSize; y++)
                {
                    Console.SetCursorPosition(X * PixelSize + x, Y * PixelSize + y);
                    Console.Write(' ');
                }
            }
        }
    }
}
