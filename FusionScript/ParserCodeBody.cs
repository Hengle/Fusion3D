﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FusionScript.Structs;
using System.IO;
namespace FusionScript
{
    public partial class Parser
    {

        public StructCode ParseCodeBody(ref int i)
        {

            Log("Begun parsing code body", i);



            var code = new StructCode();

            for (i = i; i < toks.Len; i++)
            {
                if(Get(i).Token == Token.End)
                {
                    return code;
                }
                if(Get(i).Token == Token.Else)
                {

                    return code;

                }
                var st = Predict(i);
                Console.WriteLine("PCD:" + st);
                switch (st)
                {
                    case StrandType.Return:
                        //Console.WriteLine("RET:");
                        i = NextToken(i, Token.Return);
                        var cl_return = new StructReturn();
                        if (Get(i+1).Token != Token.EndLine)
                        {
                            i++;
                            cl_return.ReturnExp = ParseExp(ref i);
                        }
                        code.Lines.Add(cl_return);
                        break;
                    case StrandType.For:

                        i = NextToken(i, Token.For);

                        i += 2;

                        var cl_for = new StructFor();

                        cl_for.Initial = ParseAssign(ref i);
                        i++;
                        cl_for.Condition = ParseExp(ref i);
                        i++;
                        cl_for.Inc = ParseAssign(ref i);
                        i++;
                        cl_for.Code = ParseCodeBody(ref i);

                        code.Lines.Add(cl_for);

                        break;
                    case StrandType.While:

                        i = NextToken(i, Token.While);

                        i += 2;

                        var cl_while = new StructWhile();

                        cl_while.Condition = ParseExp(ref i);

                        cl_while.Code = ParseCodeBody(ref i);

                        code.Lines.Add(cl_while);

                        break;
                    case StrandType.If:

                        i = NextToken(i, Token.If);

                        var cl_if = new StructIf();

                        i += 2;

                        cl_if.Condition = ParseExp(ref i);

                        cl_if.TrueCode = ParseCodeBody(ref i);


                        //    i++;
                        while (true)
                        {
                            if (Get(i).Token == Token.Else)
                            {
                                i++;
                                if (Get(i).Token == Token.If)
                                {
                                    i = i + 2;
                                    cl_if.ElseIf.Add(ParseExp(ref i));
                                    cl_if.ElseIfCode.Add(ParseCodeBody(ref i));
                                }
                                else
                                {
                                    cl_if.ElseCode = ParseCodeBody(ref i);
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                            //else
                            
                     
                                //Console.WriteLine("Else");
                            //}
                        //}/

                        code.Lines.Add(cl_if);

                        //Console.WriteLine("IF");

                        break;
                    case StrandType.Assignment:

                        Log("Parsing assignment.", i);

                        i = NextToken(i, Token.Id);

                        var assign = ParseAssign(ref i);

                        code.Lines.Add(assign);

                        break;
                    case StrandType.ClassStatement:
                        Log("Parsing class-statement.", i);
                        i = NextToken(i, Token.Id);

                        var class_state = ParseClassStatement(ref i);

                        code.Lines.Add(class_state);

                        break;
                    case StrandType.FlatStatement:
                        Log("Parsing flat-statement.", i);
                        i = NextToken(i, Token.Id);

                        if(Get(i).Text == "stop")
                        {
                            code.Lines.Add(new StructStop());
                            i++;
                            continue;
                        }

                        var flat_state = ParseFlatStatement(ref i);

                        code.Lines.Add(flat_state);

                        break;
                }


            }


            return code;

        }


    }
}
