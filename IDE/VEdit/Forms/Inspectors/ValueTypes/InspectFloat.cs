﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VividEdit.Forms.Inspectors.ValueTypes
{
    public partial class InspectFloat : UserControl
    {
        public float Value = 0;
        public InspectFloat()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Value = float.Parse(floatBox.Text);
            } catch (Exception ex)
            { 
                Value = 0;
            }
        }
        public void AlignToValue()
        {
            floatBox.Text = Value.ToString();
        }
    }
}