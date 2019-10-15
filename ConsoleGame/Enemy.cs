using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    class Enemy : ArmedEntity
    {
        public Enemy(int x, int y)
        {
            Skin =
                "****\n" +
                "  #***\n" +
                "****\n";
            //"    _..._\n" +
            //"  .\'     \'.\n" +
            //" / \\     / \\\n" +
            //"(O |     | o)\n" +
            //"(```  =  ```)\n" +
            //" \\         /\n" +
            //"  \\  ___ /\n" +
            //"   \'.___.\'\n";

            PreciseX = x;
            PreciseY = y;
            BulletSpeed = -25;
            FireRate = 0.5;
            BulletSkin = "O";
            BulletDamage = 5;
            Speed = -5;
        }
    }
}
