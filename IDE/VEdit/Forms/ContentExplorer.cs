﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo;
using WeifenLuo.WinFormsUI;
using WeifenLuo.WinFormsUI.Docking;
namespace VividEdit.Forms
{
    public delegate void OpenFile(ContentFile file);
    public partial class ContentExplorer : DockContent
    {

        public Bitmap IconFile;
        public Bitmap IconFolder;
        public List<ContentBase> Content = new List<ContentBase>();

        public static ContentExplorer Main = null;

        public static OpenFile FileOpen;

        public ContentExplorer()
        {

            InitializeComponent();
            Main = this;

        }
        public void SetFolder(string path)
        {
            contentPane.SetFolder(path);
        }
    }
}