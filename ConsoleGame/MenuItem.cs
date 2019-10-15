using System;

namespace ConsoleGame
{
    public class MenuItem : IMenuItem
    {
        public string Title { get; set; }
        public Action Action { get; set; }
    }
}