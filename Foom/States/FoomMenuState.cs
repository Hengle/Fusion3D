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
namespace Foom.States
{
    public class InvaderMenuState : FusionState
    {

        public FusionEngine.Audio.VSoundSource MenuSongSrc;
        public VSound MenuSongSound;
        public override void InitState()
        {
            base.InitState();

            MenuSongSrc = new VSoundSource("Foom/Song/menu1.mp3");
           //e2
            MenuSongSound = MenuSongSrc.Play2D(true);


            SUI = new FusionEngine.Resonance.UI();

            var TitleBG = new ImageForm().Set(0, 0, AppInfo.W, AppInfo.H).SetImage(new Texture2D("Foom/Img/titlebg1.jpg", LoadMethod.Single, false));

            var foomLab = new ImageForm().Set(AppInfo.W / 2 - 350, 40, 700, 356).SetImage(new Texture2D("Foom/Img/foom1.png", LoadMethod.Single, true));

            TitleBG.Add(foomLab);

            var StartGame = new ButtonForm().Set(AppInfo.W/2-120, 380, 260, 40, "Begin...");
            var ExitGame = new ButtonForm().Set(AppInfo.W/2-120, 430, 260, 40, "Leave...");

            TitleBG.Add(StartGame);
            TitleBG.Add(ExitGame);

            SUI.Root.Add(TitleBG);

            StartGame.Click = (b) =>
            {
                MenuSongSound.Stop();
                FusionApp.PushState(new IntroState(), true);

            };


        }

        public override void UpdateState()
        {
            base.UpdateState();
            SUI.Update();
        }

        public override void DrawState()
        {
            base.DrawState();
            SUI.Render();
        }


    }
}
