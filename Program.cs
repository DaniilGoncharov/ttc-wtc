using System;

namespace ttc_wtc
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Tokyo Tarot Cards : When They Cry";
            Console.CursorVisible = false;
            Console.SetWindowSize(90, 34);
            Console.SetBufferSize(90, 34);
            CollectedMaps.Initialise();
        }
    }
}
