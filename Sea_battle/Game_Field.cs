using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sea_battle
{
    public class Game_Field
    {
        readonly Animation conAnim = new Animation();
        public int?[,] animation;
        public Color[] status_colors = { Color.Aqua, Color.DarkBlue, Color.DarkGreen, Color.DarkOrange, Color.DarkRed };
        // 0 - поле, 1 - мимо, 2 - корабль, 3 - попали, 4 - убили;
        public Color[,] field_color; // цвета на поле
        public int[,] field_conditon; // 
        public List<Ship> ships;
        public Game_Field()
        {
            this.animation = new int? [10,10];
            this.ships = new List<Ship>();
            this.field_color = new Color[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                    field_color[i, j] = status_colors[0];
            }
            this.field_conditon = new int[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                    field_conditon[i, j] = 0;
            }
        }
        public bool New_ship_pozition_check(Point coord, int new_ship_size, bool new_ship_orientation)
        {
            if (new_ship_orientation == true)
            {
                if (coord.X + new_ship_size - 1 < 10 && coord.X + new_ship_size - 1 >= 0)
                {
                    for (int i = (coord.X == 0) ? 0 : coord.X - 1; i <= ((coord.X + new_ship_size > 9) ? 9 : coord.X + new_ship_size); i++)
                    {
                        for (int j = (coord.Y == 0) ? 0 : coord.Y - 1; j <= ((coord.Y == 9) ? 9 : coord.Y + 1); j++)
                        {
                            if (field_conditon[i, j] != 0)
                            {
                                return false;
                            }
                        }
                    }
                }
                else return false;
            }
            else
            {
                if (coord.Y + new_ship_size - 1 < 10 && coord.Y + new_ship_size - 1 >= 0)
                {
                    for (int i = (coord.X == 0) ? 0 : coord.X - 1; i <= ((coord.X == 9) ? 9 : coord.X + 1); i++)
                    {
                        for (int j = (coord.Y == 0) ? 0 : coord.Y - 1; j <= ((coord.Y + new_ship_size > 9) ? 9 : coord.Y + new_ship_size); j++)
                        {
                            if (field_conditon[i, j] != 0)
                            {
                                return false;
                            }
                        }
                    }
                }
                else return false;
            }
            return true;
        }
        public void Set_ship(Ship ship)
        {
            this.ships.Add(ship);
            for (int i = 0; i < ship.size; i++)
            {
                if (ship.orientation) // горизонтальное расположение
                {
                    this.field_conditon[ship.pozition.X + i, ship.pozition.Y] = 2;
                    this.field_color[ship.pozition.X + i, ship.pozition.Y] = this.status_colors[2];
                }
                else
                {
                    this.field_conditon[ship.pozition.X, ship.pozition.Y + i] = 2;
                    this.field_color[ship.pozition.X, ship.pozition.Y + i] = this.status_colors[2];
                }
            }
        }
        public void Random_field_generation()
        {
            Random random = new Random();
            int[] sizes = new int[] { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
            for (int i = 0; i < 10; i++)
            {
                while (true)
                {
                    int size = sizes[i];
                    int X = random.Next(0, 10);
                    int Y = random.Next(0, 10);
                    bool orientation = (random.Next(0, 2) == 0 ? true : false);
                    if (New_ship_pozition_check(new Point(X, Y), size, orientation))
                    {
                        Ship ship = new Ship(size, new Point(X, Y), orientation);
                        Set_ship(ship);
                        break;
                    }
                }
            }
        }
        public void Clear()
        {
            this.field_conditon = new int[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    field_conditon[i, j] = 0;
                    field_color[i, j] = status_colors[0];
                }
            }
            ships.Clear();
            
        }
        public bool Field_check()
        {

            int one = 0, two = 0, three = 0, four = 0;
            foreach (Ship s in ships)
            {
                switch (s.size)
                {
                    case 1: one++; break;
                    case 2: two++; break;
                    case 3: three++; break;
                    case 4: four++; break;
                }
            }
            if (one == 4 && two == 3 && three == 2 && four == 1)
                return true;
            else
                return false;
        } // проверка на количество кораблей перед игрой
        public bool Defeat_check() // true - поражение
        {
            foreach (Ship s in ships)
                if (s.Life_check())
                    return false;
            return true;
        }
        public bool Click_check(Point shoot) // true - попадание
        {
            if (field_conditon[shoot.X, shoot.Y] == 0) // промах
            {
                animation[shoot.X, shoot.Y] = 1;
                field_conditon[shoot.X, shoot.Y] = 1;
                field_color[shoot.X, shoot.Y] = status_colors[1];
                return false;
            }
            else
            {
                if (field_conditon[shoot.X, shoot.Y] == 2) // попадание в корабль
                {
                    animation[shoot.X, shoot.Y] = 1;
                    foreach (Ship s in ships)
                    {
                        if (s.Hit(shoot)) // определили корабль, в который попали
                        {

                            for (int i = 0; i < s.size; i++)
                            {
                                //обновление цветов состояния корабля
                                field_color[s.coordinates[i].X, s.coordinates[i].Y] = status_colors[s.condition[i] + 2];
                            }
                        }
                    }
                }
                return true; // все случаи кроме промаха
            }
        }
    }
}
