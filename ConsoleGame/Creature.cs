using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace ConsoleGame
{
    abstract class Creature : GameEntity
    {
        public int Speed { get; set; } = 15;
        public int Health { get; set; } = 100;
    }
}