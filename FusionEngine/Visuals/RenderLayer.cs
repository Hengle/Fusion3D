﻿using Fusion3D.Data;

namespace Fusion3D.Visuals
{
    public class RenderLayer
    {
        public RenderLayer ( )
        {
            Init ( );
        }

        public virtual void Init ( )
        {
        }

        public virtual void Bind ( Mesh3D m, Visualizer v )
        {
        }

        public virtual void Render ( Mesh3D m, Visualizer v )
        {
        }

        public virtual void Release ( Mesh3D m, Visualizer v )
        {
        }
    }
}