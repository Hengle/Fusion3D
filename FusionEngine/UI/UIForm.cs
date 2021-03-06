﻿using OpenTK;
using System.Collections.Generic;
using FusionEngine.Draw;
using FusionEngine.Font;
using FusionEngine.Logic;
using FusionEngine.Texture;

namespace FusionEngine.Resonance
{
    public delegate void Draw ( );

    public delegate void Update ( );

    public delegate void MouseEnter ( );

    public delegate void MouseLeave ( );

    public delegate void MouseMove ( int x, int y, int mx, int my );

    public delegate void MouseDown ( int but );

    public delegate void MouseUp ( int but );

    public delegate void MousePressed ( int but );

    public delegate void FormLogic ( );

    public delegate void Click ( int b );

    public delegate void Activate ( );

    public delegate void Deactivate ( );

    public delegate void KeyPressed ( OpenTK.Input.Key key, bool shift );

    public delegate void Drag ( int x, int y );

    public delegate void ChangedInfo ( );

    

    public class UIForm
    {
        public bool CheckBounds = true;
        public bool PushArea = false;
        public Logics Logics = new Logics();
        public Logics Graphics = new Logics();

        public Vector4 Col = new Vector4(1, 1, 1, 0.7f);
        public float Blur = 0.4f;
        public float RefractV = 0.4f;

        public ChangedInfo Changed = null;
        public ChangedInfo SubChanged = null;
        public Draw Draw = null;
        public Update Update = null;
        public MouseEnter MouseEnter = null;
        public MouseLeave MouseLeave = null;
        public MouseMove MouseMove = null;
        public MouseDown MouseDown = null;
        public MouseUp MouseUp = null;
        public MousePressed MousePressed = null;
        public FormLogic FormLogic = null;
        public Click Click = null;
        public Drag Drag = null;
        public Click DoubleClick = null;
        public Drag PostDrag = null;
        public Activate Activate = null;
        public Deactivate Deactivate = null;
        public Texture2D CoreTex = null;
        public Texture2D NormTex = null;
        public KeyPressed KeyPress = null;
        public int X = 0, Y = 0;
        public int W = 0, H = 0;
        public string Text = "";

        public UIForm Root = null;
        public List<UIForm> Forms = new List<UIForm>();

        public bool Peak = false;
        public bool Refract = false;

        public UIForm SetPeak ( bool peak, bool refract )
        {
            Peak = peak;
            Refract = refract;
            return this;
        }

        public UIForm Add ( UIForm form )
        {
            Forms.Add ( form );
            form.Root = this;
            return form;
        }

        public int GX
        {
            get
            {
                int x = 0;
                if ( Root != null )
                {
                    x = x + Root.GX;
                }
                x = x + X;
                return x;
            }
        }

        public int GY
        {
            get
            {
                int y = 0;
                if ( Root != null )
                {
                    y = y + Root.GY;
                }
                y = y + Y;
                return y;
            }
        }

        public virtual void DesignUI ( )
        {
        }

        public bool InBounds ( int x, int y )
        {
            if ( x >= GX && y >= GY && x <= ( GX + W ) && y <= ( GY + H ) )
            {
                return true;
            }
            return false;
        }
        public void DrawLine(int x,int y,int x2,int y2,OpenTK.Vector4 col)
        {

            Pen2D.BlendMod = PenBlend.Solid;

            Pen2D.Line(GX+x, GY+y, GX+x2, GY+y2);

        }
        public void DrawForm ( Texture2D tex, int x = 0, int y = 0, int w = -1, int h = -1 )
        {
            Pen2D.BlendMod = PenBlend.Alpha;

            int dw = W;
            int dh = H;

            if ( w != -1 )
            {
                dw = w;
                dh = h;
            }

            Pen2D.Rect ( GX + x, GY + y, dw, dh, tex, Col *UI.BootAlpha);
        }

        public void DrawFormBlur ( Texture2D tex, int x = 0, int y = 0, int w = -1, int h = -1 )
        {
            DrawFormBlur ( tex, Blur, Col, x, y, w, h );
        }

        public void DrawFormBlurRefract ( Texture2D tex, Texture2D norm, float blur, Vector4 col, float refract, int x = 0, int y = 0, int w = -1, int h = -1 )
        {
            Pen2D.BlendMod = PenBlend.Alpha;

            int dw = W;
            int dh = H;

            if ( w != -1 )
            {
                dw = w;
                dh = h;
            }

            Texture2D btex = new Texture2D(dw, dh);

            btex.CopyTex ( GX + x, App.FusionApp.H - ( ( GY + y ) + dh ) );

            Pen2D.RectBlurRefract ( GX + x, GY + y, dw, dh, tex, btex, norm, col*UI.BootAlpha, col*UI.BootAlpha, blur, refract );

            btex.Delete ( );
        }

        public void DrawFormSolid ( Vector4 col, int x = 0, int y = 0, int w = -1, int h = -1 )
        {
            if ( w == -1 )
            {
                w = W;
                h = H;
            }
            Pen2D.Rect ( GX + x, GY + y, w, h, col *UI.BootAlpha);
        }

        public void DrawFormBlur ( Texture2D tex, float blur, Vector4 col, int x = 0, int y = 0, int w = -1, int h = -1 )
        {
            Pen2D.BlendMod = PenBlend.Alpha;

            int dw = W;
            int dh = H;

            if ( w != -1 )
            {
                dw = w;
                dh = h;
            }

            Texture2D btex = new Texture2D(dw, dh);

            btex.CopyTex ( GX + x, App.FusionApp.H - ( ( GY + y ) + dh ) );

            Pen2D.RectBlur ( GX + x, GY + y, dw, dh, tex, btex, col*UI.BootAlpha, col *UI.BootAlpha, blur );

            btex.Delete ( );
        }

        public void DrawForm ( Texture2D tex, Vector4 col, int x = 0, int y = 0, int w = -1, int h = -1 )
        {
            Pen2D.BlendMod = PenBlend.Alpha;

            int dw = W;
            int dh = H;

            if ( w != -1 )
            {
                dw = w;
                dh = h;
            }

            Pen2D.Rect ( GX + x, GY + y, dw, dh, tex, col *UI.BootAlpha);
        }

        public void DrawText ( string txt, int x, int y )
        {
            DrawText ( txt,x,y, Vector4.One *UI.BootAlpha);
        }

        public void DrawText ( string txt, int x, int y, Vector4 col )
        {
            FontRenderer.Draw ( UI.Font, txt, GX+x, GY+y , col*UI.BootAlpha );
        }

        public UIForm Set ( int x, int y, int w, int h, string text = "" )
        {
            X = x;
            Y = y;
            W = w;
            H = h;
            Text = text;
            Changed?.Invoke ( );
            if ( !designed )
            {
                designed = true;
                DesignUI ( );
            }
            return this;
        }

        private bool designed = false;

        public UIForm ( )
        {
        }

        public UIForm SetImage ( Texture2D tex, Texture2D norm = null )
        {
            CoreTex = tex;
            NormTex = norm;
            return this;
        }
    }
}