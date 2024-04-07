using System;
using CelsteEngine;
namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (Game game = new(800, 600, "test"))
            {
                game.Run();
            }
        }
    }
}