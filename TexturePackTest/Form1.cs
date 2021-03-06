﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace TexturePackTest
{
    public partial class Form1 : Form
    {
        private Vivid3D.Util.Texture.TexTree tt;
        private byte[] rd = new byte[1024*1024*3];

        public Form1 ( )
        {
            InitializeComponent ( );
            tt = new Vivid3D.Util.Texture.TexTree ( 1024, 1024 );
            tm = new Bitmap ( 1024, 1024 );
            pictureBox1.Image = tm;
        }

        private Bitmap tm;

        private void button1_Click ( object sender, EventArgs e )
        {
            openFileDialog1.ShowDialog ( );
            Bitmap bm = new Bitmap(openFileDialog1.FileName);
            //pictureBox1.Image = bm;

            //pictureBox1.Invalidate ( );
            Vivid3D.Util.Texture.TreeLeaf tl = tt.Insert ( bm.Width, bm.Height );
            for ( int y = 0; y < bm.Height; y++ )
            {
                for ( int x = 0; x < bm.Width; x++ )
                {
                    //int loc = (y*1024*3)+x*3;
                    //int loc2 = (((int)tl.RC.Y+y)*1024*3)+((int)(tl.RC.X+x)*3);
                    Color cv = bm.GetPixel(x,y);

                    tm.SetPixel ( ( int ) tl.RC.X + x, ( int ) tl.RC.Y + y, cv );
                }
            }
            pictureBox1.Invalidate ( );
        }

        private void button2_Click ( object sender, EventArgs e )
        {
            openFileDialog1.ShowDialog ( );
            string file = openFileDialog1.FileName;
            Bitmap os = new Bitmap(file);
            Random rnd = new Random(Environment.TickCount);
            for ( int i = 0; i < 10000; i++ )
            {
                int nw = rnd.Next(8,256);
                int nh = rnd.Next(8,256);
                Bitmap nbp = new Bitmap(os,new Size(nw,nh));
                Vivid3D.Util.Texture.TreeLeaf tn = tt.Insert ( nw, nh );
                if ( tn == null )
                {
                    continue;

                    pictureBox1.Invalidate ( );
                    return;
                }
                for ( int y = 0; y < nh; y++ )
                {
                    for ( int x = 0; x < nw; x++ )
                    {
                        Color cv = nbp.GetPixel(x,y);
                        tm.SetPixel ( ( int ) tn.RC.X + x, ( int ) tn.RC.Y + y, cv );
                    }
                }
            }
            pictureBox1.Invalidate ( );
        }
    }
}