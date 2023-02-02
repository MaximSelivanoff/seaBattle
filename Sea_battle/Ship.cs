using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sea_battle
{
    public class Ship
    {
        readonly public int size;
        readonly public bool orientation; // true - горизонтально
        readonly public Point[] coordinates; // кооринаты клеток корабля на поле
        public int[] condition; // 0 - жив, 1 - ранен, 2 - убит
        readonly public Point pozition; // координаты верхней левой клетки корабля
        public Ship(int size, Point pozition, bool orientation)
        {
            this.orientation = orientation;
            this.pozition = pozition;
            this.size = size;
            this.condition = new int[size];
            this.coordinates = new Point[size];
            if (orientation)
                for (int i = 0; i < size; i++)
                {
                    coordinates[i] = new Point(pozition.X + i, pozition.Y);
                    condition[i] = 0;
                }
            else
                for (int i = 0; i < size; i++)
                {
                    coordinates[i] = new Point(pozition.X, pozition.Y + i);
                    condition[i] = 0;
                }
        }
        public bool Hit(Point shoot) // проверка попадания по краблю, true - попал
        {
            for (int i = 0; i < size; i++)
                if (coordinates[i] == shoot && condition[i] == 0)
                {
                    condition[i] = 1;
                    if (!Life_check())
                    {
                        for (int j = 0; j < size; j++)
                            condition[j] = 2;
                    };
                    return true;
                }
            return false;
        }
        public bool Life_check() // true - жив
        {
            bool life = false;
            for(int i = 0; i< size;i++)
            {
                if (condition[i] == 0)
                    life = true;
            }
            return life;
        }
    }
}
