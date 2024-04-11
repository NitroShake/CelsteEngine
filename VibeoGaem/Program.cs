using System;
using OpenTK.Mathematics;
using CelsteEngine;

namespace VibeoGaem
{
    internal class Program
    {

        static void Main(string[] args)
        {
            using (Game game = new(800, 600, "Celste Engine - It Just Works^TM (not actually trademarked please do not sue me thank you)"))
            {
                game.Run();
            }
        }
    }
}