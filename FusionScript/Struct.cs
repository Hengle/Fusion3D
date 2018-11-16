﻿using System.Collections.Generic;

namespace FusionScript
{
    public enum StructType
    {
        Entry, Module, Method, Function, Exit, Unknown
    }

    public delegate void ParseStructToken ( CodeToken t );

    public class Struct
    {
        public StructType Type = StructType.Unknown;
        public string Name = "";
        public List<Struct> Structs = new List<Struct>();
        public int At = 0;
        public bool Ran = false;
        public long RunCount = 0;
        public ParseStructToken PreParser = null;
        public ParseStructToken Parser = null;
        public bool Done = false;
        public TokenStream TokStream = null;

        public CodeScope LocalScope = null;

        public virtual dynamic Exec ( )
        {
            return null;
        }

        public virtual string DebugString ( )
        {
            return "Empty";
        }

        public Struct()
        {

        }

        public Struct ( TokenStream toks, bool noParse = false )
        {
            TokStream = toks;
            if ( noParse )
            {
                return;
            }

            Parse ( );
        }

       

        public virtual CodeToken BackOne ( )
        {
            return null;
            TokStream.Pos--;
            if ( TokStream.Pos < 0 )
            {
                TokStream.Pos = 0;
            }
            return TokStream.Tokes [ TokStream.Pos ];
        }

        public virtual void SkipOne ( )
        {
            TokStream.Pos++;
            if ( TokStream.Pos > TokStream.Len - 1 )
            {
                TokStream.Pos = TokStream.Len - 1;
            }
        }

        public virtual CodeToken PeekNext ( )
        {
            if ( TokStream.Pos >= TokStream.Len )
            {
                return null;
            }

            return TokStream.Tokes [ TokStream.Pos ];
        }

        public virtual CodeToken Peek ( int c )
        {
            c = c - 1;
            if ( TokStream.Pos + c >= TokStream.Len )
            {
                return null;
            }
            if (TokStream.Pos + c < 0)
            {
                return new CodeToken(TokenClass.BeginLine, Token.BeginLine, "");
            }
            return TokStream.Tokes [ TokStream.Pos + c ];
        }

        public virtual void PrevBegin ( )
        {
            for ( int i = TokStream.Pos - 1; i == 0; i-- )
            {
                if ( TokStream.Tokes [ i ].Class == TokenClass.BeginLine )
                {
                    TokStream.Pos = i;
                    return;
                }
            }
        }

        public virtual void NextBegin ( )
        {
            for ( int i = TokStream.Pos; i < TokStream.Len; i++ )
            {
                if ( TokStream.Tokes [ i ].Class == TokenClass.BeginLine )
                {
                    TokStream.Pos = i;
                    return;
                }
            }
        }

        public virtual void Parse()
        {

            SetupParser();

            TokStream.GotoPrev(Token.BeginLine);

            while (TokStream.Pos < TokStream.Len)
            {

                var toke = TokStream.GetNext();

                Parser(toke);

            

            }
        }
        public virtual void Parse2 ( )
        {
            SetupParser ( );
            if ( PreParser != null )
            {
                PreParser ( TokStream.GetNext ( ) );
            }

            while ( TokStream.Pos < TokStream.Len )
            {
                CodeToken nt = PeekNext();
                // Console.WriteLine("VS:" + nt.Text + " T:" + nt.Token);
                Trace = Trace + TokStream.Tokes [ TokStream.Pos ];
                TextTrace = TextTrace + " " + TokStream.Tokes [ TokStream.Pos ].Text;
                if ( TokStream.Tokes [ TokStream.Pos ].Class == TokenClass.BeginLine )
                {
                    if ( PeekNext ( ) == null )
                    {
                        Done = true;
                        return;
                    }
                    if(PeekNext().Token == Token.End)
                    {
                        Done = true;
                        return;
                    }
                    if(Peek(0).Token == Token.End)
                    {
                        ConsumeNext();
                       // Done = true;
                        return;
                    }
                    ConsumeNext ( );
                    if(PeekNext().Token == Token.End){
                        Done = true;
                        return;
                    }
                    if ( PeekNext ( ) == null )
                    {
                        Done = true;
                        return;
                    }
                    if ( PeekNext ( ).Class == TokenClass.BeginLine )
                    {
                        ConsumeNext ( );
                    }
                }
                if ( PeekNext ( ) == null )
                {
                    Done = true;
                    return;
                }
                if ( PeekNext ( ).Class == TokenClass.BeginLine )
                {
                    if ( TokStream.Pos >= TokStream.Len - 1 )
                    {
                        Done = true;
                        return;
                    }
                }
                Parser ( TokStream.GetNext ( ) );
                if ( Done )
                {
                    return;
                }
            }
        }

        public string Trace = "";
        public string TextTrace = "";

        public CodeToken ConsumeNext ( )
        {
            return TokStream.GetNext ( );
        }

        public virtual void SetupParser ( )
        {
        }

        public StrandType Predict ( )
        {
         
            int cpos = TokStream.Pos;
            if ( cpos >= TokStream.Len - 1 )
            {
                return StrandType.Unknown;
            }
            int imod=0;
            if ( TokStream.Tokes [ cpos ].Token == Token.LeftPara )
            {
                imod = -1;
            }
            bool ca = false;

            for ( int i = cpos; i < TokStream.Len; i++ )
            {
                int ni = i + imod;
                if ( TokStream.Tokes [ i ].Token == Token.If )
                {
                    return StrandType.If;
                }
                if ( TokStream.Tokes [ ni ].Token == Token.For )
                {
                    //System.Console.WriteLine ( "Yep!" );
                    return StrandType.For;
                }
                if ( TokStream.Tokes [ ni ].Token == Token.While )
                {
                    //System.Console.WriteLine ( "While!" );
                    return StrandType.While;
                }
                if ( TokStream.Tokes [ ni ].Token == Token.Equal )
                {
                    return StrandType.Assignment;
                }
                if(TokStream.Tokes[ni].Text == ".")
                {
                    ca = true;
                }
                switch ( TokStream.Tokes [ ni ].Class )
                {
                    case TokenClass.Id:
                        break;

                    case TokenClass.Scope:
                        if (ca)
                        {
                            return StrandType.ClassStatement;
                        }
                        else { 
                            return StrandType.FlatStatement;
                         //   break;
                        }
                        break;
                }
               // System.Console.WriteLine ( "Tok:" + TokStream.Tokes [ ni ].Class.ToString ( ) + " 2:" + TokStream.Tokes [ ni ].Token.ToString ( ) + " 3:" + TokStream.Tokes [ ni ].Text );
            }

            return StrandType.Unknown;
        }
    }

    public enum StrandType
    {
        Statement, Assignment, Flow, Define, Macro, Header, Extends, Generic, Unknown, While, If, Else, ElseIf, Wend, For, Do, Loop,
        FlatStatement,ClassStatement
    }
}