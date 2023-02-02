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

    public partial class Form1 : Form
    {

        GameMenu menu;

        public Form1()
        {
            InitializeComponent();
            this.menu = new GameMenu(this.BotPanel, this.PlayerPanel, this.groupBox1, this.button1, this.button2, this.button3);
            this.menu.TimerEnabled = true;
        }
    }
}
