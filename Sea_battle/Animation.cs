using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sea_battle
{
    public class Animation
    {
        public Image[] frames = new Image[10];
        public Animation()
        {

    
            string path = Directory.GetCurrentDirectory()+ @"\Shoot_pics\";
            for (int i = 0; i < 9; i++)
            {
                string imgPath = path + i.ToString() + ".png";
                FileStream fs0 = new FileStream(imgPath, FileMode.Open);
                Image img = Image.FromStream(fs0);
                Console.WriteLine(imgPath);
                fs0.Close();
                frames[i]=img;
            }
        }
        public Image Get_frame(int number)
        {
            return frames[number];
        }
        
    }
}
