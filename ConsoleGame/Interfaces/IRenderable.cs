using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    interface IRenderable
    {
        string Skin { get; set; }
        Hashtable SkinCoordinates { get; set; }

        double PreciseX { get; set; }
        double PreciseY { get; set; }

        int Y { get; }
        int X { get; }

        int Height { get; }
        int Width { get; }

        char GetSkinChar(int x, int y);
        bool IsEntityInBounds(int x, int y);
    }
}
