using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Sea_battle
{
    public class Player
    {
        public Panel panel;
        public Game_Field field;
        static Size panel_size = new Size(401, 401); // размер поля, в конце +1
        readonly public Size size = new Size((panel_size.Width) / 10, (panel_size.Height) / 10); // размер ячейки
        Graphics g;
        BufferedGraphics buf;
        BufferedGraphicsContext contex;
        public Animation animation;
        public Player(Panel panel)
        {
            this.panel = panel;
            panel.Size = panel_size;
            animation = new Animation();
            this.field = new Game_Field();
            this.contex = BufferedGraphicsManager.Current;
            this.buf = contex.Allocate(this.panel.CreateGraphics(), this.panel.DisplayRectangle);
            this.g = this.buf.Graphics;
        }
        public void CreateMap()
        {
            Pen pen = new Pen(Color.Black, 1);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    SolidBrush kistb = new SolidBrush(field.field_color[i, j]);
                    Point point = new Point(i * size.Width, j * size.Height);
                    Rectangle rectangle = new Rectangle(point, size);
                    this.g.FillRectangle(kistb, rectangle);
                    this.g.DrawRectangle(pen, rectangle);
                    Image frame;
                    if (field.animation[i, j] != null)
                    {
                        frame = animation.Get_frame(field.animation[i, j].Value);
                        if (field.animation[i, j] + 1 != 10)
                        {
                            this.g.DrawImage(frame, point.X, point.Y, size.Width, size.Height);
                            field.animation[i, j]++;
                        }
                        else
                            field.animation[i, j] = null;
                    }
                }
            }
            this.buf.Render();
        }

    }
}

