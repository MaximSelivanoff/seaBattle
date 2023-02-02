using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sea_battle
{
    class GameMenu
    {
        Bot Bot;
        Player Player;
        Timer timer;
        GroupBox GroupBox;
        ListBox listBox1;
        RadioButton radioButton1;
        RadioButton radioButton2;
        Button Button, Button2, Button3;
        public bool Player_turnn;
        public int? new_ship_size;
        public bool? new_ship_orientation;
        bool activate;
        Timer Bot_timer = new Timer { Interval = 250 };
        public GameMenu(Panel Bot_panel, Panel Player_panel, GroupBox GroupBox, Button button, Button button2, Button button3)
        {
            this.activate = true;
            this.GroupBox = GroupBox;
            this.Bot = new Bot(Bot_panel);
            this.Player = new Player(Player_panel);
            this.timer = new Timer { Interval = 30 };
            this.timer.Tick += delegate (object sender, EventArgs e)
            {
                this.Bot.CreateMap();
                this.Player.CreateMap();
            };
            this.Player.panel.MouseClick += this.Get_ship;
            listBox1 = (ListBox)this.GroupBox.Controls.Find("listBox1", false)[0];
            radioButton1 = (RadioButton)this.GroupBox.Controls.Find("radioButton1", false)[0];
            radioButton2 = (RadioButton)this.GroupBox.Controls.Find("radioButton2", false)[0];
            listBox1.SelectedIndexChanged += this.ListBoxMouseClick;
            this.Button = button;
            this.Button2 = button2;
            this.Button3 = button3;
            this.Button.Click += this.Start_game;
            this.Button2.Click += this.Rand_fill;
            this.Button3.Click += this.Clear_field;
            this.radioButton1.MouseClick += delegate
            {
                this.new_ship_orientation = true;
                this.radioButton1.Checked = true;
                this.radioButton2.Checked = false;
            };
            this.radioButton2.MouseClick += delegate
            {
                this.new_ship_orientation = false;
                this.radioButton1.Checked = false;
                this.radioButton2.Checked = true;
            };
        }
        private void ListBoxMouseClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;
            switch (listBox1.SelectedItem.ToString())
            {
                case "Однопалубный корабль":
                    new_ship_size = 1;
                    break;
                case "Двухпалубный корабль":
                    new_ship_size = 2;
                    break;
                case "Трехпалубный корабль":
                    new_ship_size = 3;
                    break;
                case "Четырехпалубный корабль":
                    new_ship_size = 4;
                    break;
            }
        }
        public void Get_ship(object sender, MouseEventArgs e)
        {
            if (this.listBox1.Items.Count == 0)
                return;
            if (!this.new_ship_orientation.HasValue || !this.new_ship_size.HasValue)
                return;
            Point coord = new Point();
            coord.X = e.X / Player.size.Width;
            coord.Y = e.Y / Player.size.Height;
            coord.X = (coord.X > 9) ? 9 : coord.X;
            coord.Y = (coord.Y > 9) ? 9 : coord.Y;
            Ship new_ship = null;
            if (!Player.field.New_ship_pozition_check(coord, new_ship_size.Value, new_ship_orientation.Value))
                return;
            new_ship = new Ship(new_ship_size.Value, coord, new_ship_orientation.Value);
            this.Player.field.Set_ship(new_ship);
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            listBox1.SelectedItem = null;
        }
        public void Clear_field(object sender, EventArgs e)
        {
            Player.field.Clear();
        }
        public void Rand_fill(object sender, EventArgs e)
        {
            Player.field.Clear();
            Player.field.Random_field_generation();
        }
        public void Start_game(object sender, EventArgs e)
        {
            if (Button.Text == "Стартб")
            {
                if (!Player.field.Field_check())
                {
                    MessageBox.Show("Неверное расположение кораблей");
                    return;
                }
                else
                {
                    MessageBox.Show("Пагнале!!");
                    Button.Text = "Сдаться";
                    Button2.Enabled = false;
                    Button3.Enabled = false;
                    listBox1.Enabled = false;
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;
                    Player_turnn = false;
                    activate = true;
                    Bot_timer.Tick += game; 
                    Bot_timer.Enabled = true;
                }
            }
            else
            {
                end_game();
            }
        }
        public void end_game()
        {
            Bot_timer.Enabled = false;
            Bot_timer.Tick -= game;
            Button.Text = "Стартб";
            Button2.Enabled = true;
            Button3.Enabled = true;
            listBox1.Enabled = true;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            Bot.panel.MouseClick -= Bot.Player_turn;
            if (Player_turnn)
                MessageBox.Show("Юхуууу! Победа, оаоаоаоаоо!!!!");
            else
                MessageBox.Show("Капес(");
            Bot.field.Clear();
            Bot.field.Random_field_generation();
            Player.field.Clear();
        }
        public void game(object sender2, EventArgs e2)
        {
            if (Player_turnn)
            {
                if (this.activate)
                {
                    activate = false;
                    Bot.Player_stop = false;
                    Bot.panel.MouseClick += Bot.Player_turn;
                }
                if (Bot.Player_stop)
                {
                    activate = true;
                    Player_turnn = false;
                    Bot.panel.MouseClick -= Bot.Player_turn;
                }
                if (Bot.field.Defeat_check() && false)
                {
                    Player_turnn = true;
                    end_game();
                    return;
                };
            }
            else
            {
                if (!Player.field.Click_check(Bot.Rand_Shoot()))
                {
                    Player_turnn = true;
                    if (Player.field.Defeat_check())
                    {
                        Player_turnn = false;
                        end_game();
                        return;
                    }
                }
            }
        }

        public bool TimerEnabled
        {
            set => this.timer.Enabled = value;
            get => this.timer.Enabled;  
        }
    }
}
