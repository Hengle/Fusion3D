﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FusionEngine;
using FusionEngine.Resonance;
using FusionEngine.Resonance.Forms;
using FusionEngine.State;
using FusionEngine.App;
using FusionEngine.Audio;
using FusionEngine.Texture;
namespace FoomED.States
{
    public class MainMenuState : FusionState
    {


        public override void InitState()
        {
            base.InitState();

            var win = new WindowForm().Set(50, 50, 450, 300, "FoomED");

            var bg = new ImageForm().Set(0, 0, AppInfo.W, AppInfo.H, "").SetImage(new Texture2D("FoomED/bg1.jpg", LoadMethod.Single, false));

            SUI = new UI();

            var new_map = new ButtonForm().Set(10, 40, 120, 30, "New Map");

            win.Add(new_map);

            SUI.Root.Add(bg);

            SUI.Root.Add(win);

            new_map.Click = (b) =>
            {

                FusionApp.PushState(new NewMapState());

            };

        }

        public override void UpdateState()
        {
            base.UpdateState();

            SUI.Update();
        }

        public override void DrawState()
        {
           
        
        
            base.ResumeState();

            SUI.Render();
        }

    }
}
