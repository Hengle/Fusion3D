﻿using System;

namespace VividScript.VStructs
{
    public class VSAssign : VStruct
    {
        public System.Collections.Generic.List<VSVar> Vars = new System.Collections.Generic.List<VSVar>();
        public System.Collections.Generic.List<VSExpr> Expr = new System.Collections.Generic.List<VSExpr>();

        public VSAssign ( VTokenStream s ) : base ( s )
        {
        }

        public bool CreatedVars = false;

        public override dynamic Exec ( )
        {
            if ( !CreatedVars )
            {
                VSVar qv = VME.CurrentScope.FindVar( Vars[0].Name,true );
                if ( qv != null )
                {
                    Vars [ 0 ] = qv;
                }
                else
                {
                    VME.CurrentScope.RegisterVar ( Vars [ 0 ] );
                }
            }
            Vars[0].Value = Expr[0].Exec();
            return null;
        }

        public override void SetupParser ( )
        {
            Parser = ( t ) =>
            {
                string name = t.Text;
                if ( t.Text == "=" )
                {
                    t = BackOne ( );
                    t = BackOne ( );
                    name = t.Text;
                    ConsumeNext ( );
                }
                VSVar nv = new VSVar
                {
                    Name = name
                };
                Vars.Add ( nv );

                switch ( t.Token )
                {
                    case Token.Id:
                        if ( PeekNext ( ).Token == Token.Equal )
                        {
                            Console.WriteLine ( "=" );
                            ConsumeNext ( );
                            Expr.Add ( new VSExpr ( TokStream ) );
                            BackOne ( );
                            if ( PeekNext ( ).Token == Token.EndLine || PeekNext ( ).Token == Token.RightPara )
                            {
                                Done = true;
                                return;
                            }
                        }
                        break;

                    case Token.End:
                    case Token.EndLine:
                        Done = true;
                        return;
                        break;
                }
            };
        }
    }
}