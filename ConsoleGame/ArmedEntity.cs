using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ConsoleGame
{
    class ArmedEntity : Creature
    {
        public int BulletDamage { get; set; } = 10;
        public Point GunCoordinates { get; set; }
        public char GunChar { get; set; } = '#';
        private Stopwatch ReloadSW { get; set; } = new Stopwatch();

        public double FireRate { get; set; } = 5;
        public int BulletSpeed { get; set; } = 25;
        public string BulletSkin { get; set; } = "O";

        public List<Bullet> Bullets { get; set; } = new List<Bullet>();

        public Bullet Fire()
        {
            if (ReloadSW.Elapsed.TotalSeconds > (1 / FireRate))
            {
                ReloadSW = Stopwatch.StartNew();
                ReloadSW.Start();
                if (IsGunWorking())
                {
                    var bullet = new Bullet(GunCoordinates)
                    {
                        Speed = BulletSpeed,
                        Skin = BulletSkin,
                        Damage = BulletDamage
                    };
                    Bullets.Add(bullet);
                    return bullet;
                }
            }

            return null;
        }

        public ArmedEntity()
        {
            ReloadSW.Start();
        }

        public bool IsGunWorking()
        {
            if (GunCoordinates != null)
                return true;

            return false;
        }

        public override void RecalculatePoints()
        {
            SkinCoordinates.Clear();

            int local_x = X;
            int local_y = Y;

            for (int i = 0; i < Skin.Length; i++)
            {
                if (Skin[i] == '\n')
                {
                    local_y++;
                    local_x = X;
                }
                else if (Skin[i] == GunChar)
                {
                    GunCoordinates = new Point(local_x, local_y);
                }
                else
                {
                    local_x++;
                    SkinCoordinates.Add(new Point(local_x, local_y), Skin[i]);
                }
            }
        }
    }
}
