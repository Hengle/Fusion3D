﻿namespace FusionScript.Structs
{
    public class StructIf : Struct
    {
        public StructExpr Condition;
        public StructCode TrueCode;

        public StructIf ( TokenStream s ) : base ( s )
        {
        }

        public override dynamic Exec ( )
        {
            if ( Condition.Exec ( ) )
            {
                TrueCode.Exec ( );
            }
            return null;
        }

        public override void SetupParser ( )
        {
          
        }
    }
}