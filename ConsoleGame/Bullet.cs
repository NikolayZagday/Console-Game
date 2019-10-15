using System;
using System.Windows;

namespace ConsoleGame
{
    class Bullet : Creature
    {
        public BulletBelongsTo ShotBy { get; set; } 
        public int Damage { get; set; }

        private Bullet()
        {

        }

        public Bullet(Point p) : base()
        {
            PreciseX = p.X;
            PreciseY = p.Y;
            Speed = 40;
            Skin = "+";
        }
    }
}
