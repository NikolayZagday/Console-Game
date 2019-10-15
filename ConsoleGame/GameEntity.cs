using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ConsoleGame
{
    class GameEntity : IRenderable
    {
        public string Skin { get; set; }
        public Hashtable SkinCoordinates { get; set; } = new Hashtable();

        public int X
        {
            get
            {
                return (int)Math.Round(PreciseX, MidpointRounding.AwayFromZero);
            }
        }

        private double m_x;
        public double PreciseX
        {
            get
            {
                return m_x;
            }
            set
            {
                m_x = value;
                RecalculatePoints();
            }
        }

        public int Y
        {
            get
            {
                return (int)Math.Round(PreciseY, MidpointRounding.AwayFromZero);
            }
        }

        private double m_y;
        public double PreciseY
        {
            get
            {
                return m_y;
            }
            set
            {
                m_y = value;
                RecalculatePoints();
            }
        }

        public int Height
        {
            get
            {
                int hight = Skin.Count((x) => x == '\n');

                if (hight == 0)
                    return 1;

                return hight;
            }
        }

        public int Width
        {
            get
            {
                int width = 0;

                string[] lines = Skin.Split('\n');

                foreach (var line in lines)
                {
                    if (line.Length > width)
                        width = line.Length;
                }

                return width;
            }
        }

        public virtual void RecalculatePoints()
        {
            if (Skin == null)
                return;

            SkinCoordinates.Clear();

            int local_x = X;
            int local_y = Y;

            if (Skin.Length == 1)
            {
                SkinCoordinates.Add(new Point(local_x, local_y), Skin[0]);
                return;
            }

            for (int i = 0; i < Skin.Length - 1; i++)
            {
                if (Skin[i] == '\n')
                {
                    local_y++;
                    local_x = X;
                }
                else
                {
                    local_x++;
                    SkinCoordinates.Add(new Point(local_x, local_y), Skin[i]);
                }
            }
        }

        public char GetSkinChar(int x, int y)
        {
            return (char)SkinCoordinates[new Point(x, y)];
        }

        public bool IsEntityInBounds(int x, int y)
        {
            return SkinCoordinates.Contains(new Point(x, y));
        }
    }
}