﻿namespace VividEdit.Forms
{
    partial class Editor3D
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.vivid3DDisplay1 = new VividEdit.Forms.Vivid3DDisplay();
            this.SuspendLayout();
            // 
            // vivid3DDisplay1
            // 
            this.vivid3DDisplay1.BackColor = System.Drawing.Color.Black;
            this.vivid3DDisplay1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vivid3DDisplay1.Location = new System.Drawing.Point(0, 0);
            this.vivid3DDisplay1.Name = "vivid3DDisplay1";
            this.vivid3DDisplay1.Size = new System.Drawing.Size(800, 450);
            this.vivid3DDisplay1.TabIndex = 0;
            this.vivid3DDisplay1.VSync = false;
            // 
            // Editor3D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.vivid3DDisplay1);
            this.Name = "Editor3D";
            this.Text = "Editor3D";
            this.ResumeLayout(false);

        }

        #endregion

        private VividEdit.Forms.Vivid3DDisplay vivid3DDisplay1;
    }
}