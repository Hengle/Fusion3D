﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo;
using WeifenLuo.WinFormsUI;
using WeifenLuo.WinFormsUI.Docking;
using VividEdit.Forms;
namespace VividEdit
{
    public partial class VEdit : Form
    {

        public static string BeginProject = "";
        public static ProjectCore.Project CurProject;

        public VEdit()
        {
            
            InitializeComponent();
            
            MainDock = new DockPanel();
            MainDock.Dock = DockStyle.Fill;
            this.Controls.Add(MainDock);
            // DockEdit3D = new Forms.Editor3D();
            //DockEdit3D.Show();
            // DockEdit3D.Show(MainDock, DockState.Document);
         
        }

        private void VividEdit_Load(object sender, EventArgs e)
        {
            DockAppGraph = new Forms.AppGraph();
            DockEdit3D = new Forms.Editor3D();
            DockContentExplorer = new Forms.ContentExplorer();
            DockConsoleView = new Forms.ConsoleView();
            DockAppGraph.Show(MainDock, DockState.DockLeft);
            DockConsoleView.Show(MainDock, DockState.DockBottom);
            DockContentExplorer.Show(MainDock, DockState.DockBottom);
           
            DockEdit3D.Show(MainDock, DockState.Document);

            ConsoleView.Log("Vivid Editor started up.", "IDE");

            ConsoleView.Log("Loading project:" + BeginProject, "IDE");
            CurProject = new ProjectCore.Project(BeginProject);
            ConsoleView.Log("Project loaded.", "IDE");

            ConsoleView.Log("Scanning default Content folder:"+CurProject.ContentPath, "IDE");
            DockContentExplorer.SetFolder(CurProject.ContentPath);
            ConsoleView.Log("IDE initialized.", "IDE");
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {

           
        }
    }
}