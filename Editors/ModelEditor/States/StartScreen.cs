﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D;
using Vivid3D.App;
using Vivid3D.Draw;
using Vivid3D.FX;
using Vivid3D.FXS;
using Vivid3D.Input;
using Vivid3D.Scene;
using Vivid3D.Tex;
using Vivid3D.Util;
using Vivid3D.VFX;

using Vivid3D.Reflect;
using System.Reflection;
using Vivid3D.Archive;
using Vivid3D.Lighting;
using Vivid3D.PostProcess;
using Vivid3D.Import;
using Vivid3D.Material;
using Vivid3D.State;
using ModelEditor.Forms;
namespace ModelEditor.States
{
    public class StartScreen : VividState 
    {

        public override void InitState()
        {

            InitUI();

            SUI.Root = new StartScreenForm().Set(0, 0, VividApp.W, VividApp.H);
            

        }

        public override void UpdateState()
        {

            SUI.Update();

        }


        public override void DrawState()
        {

            SUI.Render();

        }

    }
}
