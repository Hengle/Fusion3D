﻿namespace FusionEngine.Composition.FrameTypes
{
    public class FrameColor : FrameType
    {
        public override void Generate ( )
        {
            if ( Regenerate == false )
            {
                return;
            }

            BindTarget ( );

            Graph.Render ( );

            ReleaseTarget ( );
        }
    }
}