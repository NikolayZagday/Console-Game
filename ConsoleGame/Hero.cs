using System;
using System.Collections.Generic;

namespace ConsoleGame
{
    class Hero : ArmedEntity
    {
        public Hero(int bulletSpeed = 15, int bulletDamage = 10, int fireRate = 4)
        {
            Speed = 20;

            Skin =
                "   XXXXX\n" +
                "XXXX#\n" +
                "   XXXXX\n";

            Health = 1;
            BulletSpeed = bulletSpeed;
            BulletDamage = bulletDamage;
            FireRate = fireRate;
            BulletSkin = "+";
        }
    }
}