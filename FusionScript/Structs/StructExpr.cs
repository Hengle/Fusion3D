﻿using System;
using System.Collections.Generic;

namespace FusionScript.Structs
{
    public enum ExprType
    {
        IntValue, FloatValue, StringValue, RefValue, ClassValue, SubExpr, Operator, Unknown, VarValue, BoolValue,NewClass,ClassVar
    }

    public enum OpType
    {
        Plus, Minus, Divide, Times, Pow2, Percent, EqualTo, LessThan, MoreThan, NotEqual
    }

    public class StructExpr : Struct
    {
        public List<StructExpr> Expr = new List<StructExpr>();

        public ExprType Type = ExprType.Unknown;
        public OpType Op = OpType.Divide;

        public string StringV="";
        public int intV=0;
        public float floatV=0;
        public bool BoolV = false;
        public string VarName="";
        public bool NewClass = false;
        public StructCallPars NewPars;
        public string NewClassType = "";
 

        public override string DebugString ( )
        {
            return "Expr:" + Expr.Count + " Elements";
        }



        public override dynamic Exec()
        {
            if (Expr[0].NewClass)
            {

                var base_class = ManagedHost.Main.FindClass(Expr[0].NewClassType);
                var nvar = new Var();
                nvar.Name = base_class.ModuleName + "_Instnace";

                var bc = base_class.CreateInstance();

                if (Expr[0].NewPars != null)
                {
                    dynamic[] pars = new dynamic[Expr[0].NewPars.Pars.Count];
                    if (Expr[0].NewPars != null)
                    {
                        int ii = 0;
                        foreach (var pe in Expr[0].NewPars.Pars)
                        {
                            pars[ii] = pe.Exec();
                            ii++;
                        }

                        ManagedHost.Main.ExecuteMethod(bc, base_class.ModuleName, pars);
                    }
                }
                else
                {


                    ManagedHost.Main.ExecuteMethod(bc, base_class.ModuleName, null);
                }
                return bc;

            }
            //Val.Clear();
            switch (Expr[0].Type)
            {
                case ExprType.NewClass:
                    Console.WriteLine("NC");
                    break;
                case ExprType.StringValue:

                    string rs = "";
                    foreach(var se in Expr)
                    {
                        switch (se.Type)
                        {
                            case ExprType.StringValue:
                                rs = rs + se.StringV;
                                break;
                            case ExprType.SubExpr:
                                var ser = se.Exec();
                                rs = rs + se.Exec();
                                break;
                            case ExprType.FloatValue:
                                rs = rs + se.floatV.ToString();
                                break;
                            case ExprType.IntValue:
                                rs = rs + se.intV.ToString();
                                break;
                            case ExprType.VarValue:
                                var strv = ManagedHost.CurrentScope.FindVar(se.VarName, true);
                                rs = rs + strv.Value;

                                break;
                            case ExprType.ClassVar:
                                var r3 = se.Exec();
                                rs = rs + r3;
                                Console.WriteLine("!");
                                break;

                        }
                    }
                    return rs;
                    break;
                case ExprType.IntValue:

                    Stack.Clear();
                    Output.Clear();
                    ToRPNInt(Expr);
                    var resi2 = CalcInt();


                    return resi2.intV;

                    break;
                case ExprType.VarValue:
                case ExprType.ClassVar:
                   
                    dynamic varc = null;
                    int ec = 0;
                    while (true)
                    {

                        if (varc == null)
                        {
                            varc = ManagedHost.CurrentScope.FindVar(Expr[ec].Expr[0].VarName, true);
                        }
                        else
                        {

                            if (varc.Value.HasFunc(Expr[ec].VarName))
                            {
                                varc = varc.Value.ExecFunc(Expr[ec].VarName,null);

                            }else
                            {
                                varc = varc.Value.FindVar(Expr[ec].VarName);
                                if (varc == null)
                                {

                                }
                            }
                        }
                        ec++;
                        if (ec == Expr.Count)
                        {
                            break;
                        }
                    }

                    if(varc is string)
                    {
                        return varc;
                    }
                    if(varc.Type == VarType.String)
                    {

                        return varc.Value;

                    }

                    if (varc.Type == VarType.Float)
                    {
                        Stack.Clear();
                        Output.Clear();
                        ToRPNFloat(Expr);
                        var resf2 = CalcFloat();
                        return resf2.floatV;
                    }
                    else if (varc.Type == VarType.Int)
                    {
                        Stack.Clear();
                        Output.Clear();
                        ToRPNInt(Expr);
                        var res = CalcInt();


                        return res.intV;
                    }
                    break;
                case ExprType.FloatValue:

                    Stack.Clear();
                    Output.Clear();
                    ToRPNFloat(Expr);
                    var resf = CalcFloat();


                    return resf.floatV;


                    break;
            }

            return 0;

        }

        public StructExpr CalcInt()
        {
            Stack.Clear();
            for (int i = 0; i < Output.Count; i++)
            {
                if (Output[i].Type == ExprType.Operator)
                {
                    var left = Stack.Pop();
                    var right = Stack.Pop();
                    switch (Output[i].Op)
                    {
                        case OpType.LessThan:

                            var lt1 = new StructExpr();
                            lt1.intV = (left.intV > right.intV) ? 1 : 0;
                            Stack.Push(lt1);
                            break;

                        case OpType.MoreThan:

                            var mt1 = new StructExpr();
                            mt1.intV = (left.intV < right.intV) ? 1 : 0;
                            Stack.Push(mt1);
                            break;
                        case OpType.EqualTo:
                            var et1 = new StructExpr();
                            et1.intV = (left.intV == right.intV) ? 1 : 0;
                            Stack.Push(et1);
                            break;
                        case OpType.Plus:
                            var ne1 = new StructExpr();
                            ne1.intV = left.intV + right.intV;
                            Stack.Push(ne1);
                            break;
                        case OpType.Times:
                            var ne2 = new StructExpr();
                            ne2.intV = left.intV * right.intV;
                            Stack.Push(ne2);
                            break;
                        case OpType.Divide:
                            var ne3 = new StructExpr();
                            ne3.intV = left.intV / right.intV;
                            Stack.Push(ne3);
                            break;
                        case OpType.Minus:
                            var ne4 = new StructExpr();
                            ne4.intV = left.intV - right.intV;
                            Stack.Push(ne4);
                            break;
                    }
                    //switch(Output[i].)

                }
                else
                {
                    Stack.Push(Output[i]);
                }

            }
            return Stack.Peek();
          
        }


        public StructExpr CalcFloat ()
        {
            Stack.Clear();
            for (int i = 0; i < Output.Count; i++)
            {
                if (Output[i].Type == ExprType.Operator)
                {
                    var left = Stack.Pop();
                    var right = Stack.Pop();
                    switch (Output[i].Op)
                    {
                        case OpType.LessThan:

                            var lt1 = new StructExpr();
                            lt1.floatV = (left.floatV > right.floatV) ? 1 : 0;
                            Stack.Push(lt1);
                            break;

                        case OpType.MoreThan:

                            var mt1 = new StructExpr();
                            mt1.floatV = (left.floatV < right.floatV) ? 1 : 0;
                            Stack.Push(mt1);
                            break;
                        case OpType.EqualTo:
                            var et1 = new StructExpr();
                            et1.floatV = (left.floatV == right.floatV) ? 1 : 0;
                            Stack.Push(et1);
                            break;
                        case OpType.Plus:
                            var ne1 = new StructExpr();
                            ne1.floatV = left.floatV + right.floatV;
                            Stack.Push(ne1);
                            break;
                        case OpType.Times:
                            var ne2 = new StructExpr();
                            ne2.floatV = left.floatV * right.floatV;
                            Stack.Push(ne2);
                            break;
                        case OpType.Divide:
                            var ne3 = new StructExpr();
                            ne3.floatV = left.floatV / right.floatV;
                            Stack.Push(ne3);
                            break;
                        case OpType.Minus:
                            var ne4 = new StructExpr();
                            ne4.floatV = left.floatV - right.floatV;
                            Stack.Push(ne4);
                            break;
                    }
                    //switch(Output[i].)

                }
                else
                {
                    Stack.Push(Output[i]);
                }

            }
            return Stack.Peek();

        }

        List<StructExpr> Output = new List<StructExpr>();
        Stack<StructExpr> Stack = new Stack<StructExpr>();
        public StructCallPars CallPars;
    
        public void ToRPNInt(List<StructExpr> exp)
        {

            Output.Clear();
            dynamic av = null;
            for(int i = 0; i < exp.Count; i++)
            {
                switch (exp[i].Type)
                {
                    case ExprType.ClassVar:

                        dynamic basev = null;
                        foreach(var cve in exp[i].Expr)
                        {
                            if (basev == null)
                            {
                                basev = ManagedHost.CurrentScope.FindVar(cve.VarName, true);
                                if (basev == null)
                                {
                                    Console.WriteLine("No var:" + cve.VarName);
                                    while (true)
                                    {

                                    }
                                }
                            }
                            else
                            {
                                basev = basev.Value.FindVar(cve.VarName);

                            }
                        }

                        var cv = new StructExpr();
                        cv.intV = basev.Value;
                        Output.Add(cv);

                        break;
                    case ExprType.VarValue:
                        try
                        {
                            if (av == null)
                            {
                                av = ManagedHost.CurrentScope.FindVar(exp[i].VarName, true);
                                if (av.Value is StructModule || av.Value is Module)
                                {

                                }
                                else
                                {
                                    var nnn = new StructExpr();
                                    nnn.intV = (int)av.Value;
                                    Output.Add(nnn);
                                }
                            }
                            else
                            {

                                if (av.Value is Module)
                                {
                                    if (av.Value.CMod != null)
                                    {
                                        if (av.Value.CMod.HasStaticVar(exp[i].VarName))
                                        {
                                            av = av.Value.CMod.GetStaticValue(exp[i].VarName);
                                            var ne = new StructExpr();
                                            ne.intV = av;
                                            Output.Add(ne);
                                        }
                                    }
                                }
                                else if (av.Value is StructModule)
                                {
                                    if (av.Value.IsFunc(exp[i].VarName))
                                    {
                                        if (exp[i].CallPars != null)
                                        {
                                            dynamic[] pars = new dynamic[exp[i].CallPars.Pars.Count];
                                            for (int cpi = 0; cpi < exp[i].CallPars.Pars.Count; cpi++)
                                            {
                                                pars[cpi] = exp[i].CallPars.Pars[cpi].Exec();
                                            }

                                            av = av.Value.ExecFunc(exp[i].VarName, pars);
                                        }
                                        else
                                        {
                                            av = av.Value.ExecFunc(exp[i].VarName, null);
                                        }
                                            var fr = new StructExpr();
                                        fr.intV = av;
                                        Output.Add(fr);
                                    }
                                    else
                                    {
                                        av = av.Value.FindVar(exp[i].VarName);
                                        if (av.Value is int)
                                        {
                                            var ni = new StructExpr();
                                            ni.intV = av.Value;
                                            Output.Add(ni);
                                        }
                                    }
                                }

                            }
                        }
                        catch
                        {
                            Console.WriteLine("Variable retrival error.");
                            ManagedHost.RaiseError("Variable retrival error.");
                            
                        }
                        break;
                    case ExprType.SubExpr:
                        var ns = new StructExpr();
                        ns.intV = exp[i].Exec();
                        Output.Add(ns);
                        break;
                    case ExprType.IntValue:
                        Output.Add(exp[i]);
                        break;
                    case ExprType.Operator:
                        while (Stack.Count > 0 && Priority(Stack.Peek()) >= Priority(exp[i]))
                            Output.Add(Stack.Pop());
                        Stack.Push(exp[i]);
                        //{
                        //   o
                        //}
                        break;
                    
                }

            }
            while (Stack.Count > 0)
                Output.Add(Stack.Pop());
        }

        public void ToRPNFloat(List<StructExpr> exp)
        {

            Output.Clear();
            dynamic av = null;
            for (int i = 0; i < exp.Count; i++)
            {
                switch (exp[i].Type)
                {
                    case ExprType.ClassVar:

                        dynamic basev = null;
                        foreach (var cve in exp[i].Expr)
                        {
                            if (basev == null)
                            {
                                basev = ManagedHost.CurrentScope.FindVar(cve.VarName, true);
                                if (basev == null)
                                {
                                    Console.WriteLine("No var:" + cve.VarName);
                                    while (true)
                                    {

                                    }
                                }
                            }
                            else
                            {
                                basev = basev.Value.FindVar(cve.VarName);

                            }
                        }

                        var cv = new StructExpr();
                        cv.floatV = basev.Value;
                        Output.Add(cv);

                        break;
                    case ExprType.VarValue:
                        try
                        {
                            if (av == null)
                            {
                                av = ManagedHost.CurrentScope.FindVar(exp[i].VarName, true);
                                if (av.Value is StructModule || av.Value is Module)
                                {

                                }
                                else
                                {
                                    var nnn = new StructExpr();
                                    nnn.floatV = av.Value;
                                    Output.Add(nnn);
                                }
                            }
                            else
                            {

                                if (av.Value is Module)
                                {
                                    if (av.Value.CMod != null)
                                    {
                                        if (av.Value.CMod.HasStaticVar(exp[i].VarName))
                                        {
                                            av = av.Value.CMod.GetStaticValue(exp[i].VarName);
                                            var ne = new StructExpr();
                                            ne.floatV = av;
                                            Output.Add(ne);
                                        }
                                    }
                                }
                                else if (av.Value is StructModule)
                                {
                                    if (av.Value.IsFunc(exp[i].VarName))
                                    {
                                        if (exp[i].CallPars != null)
                                        {
                                            dynamic[] pars = new dynamic[exp[i].CallPars.Pars.Count];
                                            for (int cpi = 0; cpi < exp[i].CallPars.Pars.Count; cpi++)
                                            {
                                                pars[cpi] = exp[i].CallPars.Pars[cpi].Exec();
                                            }

                                            av = av.Value.ExecFunc(exp[i].VarName, pars);
                                        }
                                        else
                                        {
                                            av = av.Value.ExecFunc(exp[i].VarName, null);
                                        }
                                        var fr = new StructExpr();
                                        fr.floatV = av;
                                        Output.Add(fr);
                                    }
                                    else
                                    {
                                        av = av.Value.FindVar(exp[i].VarName);
                                        if (av.Value is int)
                                        {
                                            var ni = new StructExpr();
                                            ni.floatV = av.Value;
                                            Output.Add(ni);
                                        }
                                    }
                                }

                            }
                        }
                        catch
                        {
                            Console.WriteLine("Variable retrival error.");
                            ManagedHost.RaiseError("Variable retrival error.");

                        }
                        break;
                    case ExprType.SubExpr:
                        var ns = new StructExpr();
                        ns.floatV = exp[i].Exec();
                        Output.Add(ns);
                        break;
                    case ExprType.FloatValue:
                        Output.Add(exp[i]);
                        break;
                    case ExprType.Operator:
                        while (Stack.Count > 0 && Priority(Stack.Peek()) >= Priority(exp[i]))
                            Output.Add(Stack.Pop());
                        Stack.Push(exp[i]);
                        //{
                        //   o
                        //}
                        break;

                }

            }
            while (Stack.Count > 0)
                Output.Add(Stack.Pop());
        }

        public int ParseInt(Stack<StructExpr> vals)
        {

            StructExpr val = vals.Pop();
            int x=0, y=0;
            if ((val.Type == ExprType.Operator))
            {

                y = ParseInt(vals);
                x = ParseInt(vals);
                if (val.Op == OpType.Plus) x += y;
                else if (val.Op == OpType.Minus) x -= y;
                else if (val.Op == OpType.Times) x *= y;
                else if (val.Op == OpType.Divide) x /= y;
                else throw new Exception("Wrong expression");
            }
            else
            {
                x = val.intV; 
            }
            return x;

            
            
            
              
            

          
        }

        private static int Priority(StructExpr op)
        {
            

            if (op.Op == OpType.Pow2)
                return 4;
            if (op.Op == OpType.Times || op.Op == OpType.Divide)
                return 3;
            if (op.Op == OpType.Plus || op.Op == OpType.Minus)
                return 2;
            else
                return 1;
        }


        public dynamic NextE ( int i, dynamic prev )
        {
            dynamic val="";
            StructExpr e = Expr[i];
            if(e.VarName == ".")
            {
                val = NextE(i + 1, prev);
            }
            if (prev is StructModule && e.VarName != ".") 
            {

                val= prev.FindVar(e.VarName).Value;
            }

            switch ( e.Type )
            {
                case ExprType.SubExpr:
                    val = e.NextE ( 0, null );
                    break;

                case ExprType.Operator:
                    switch ( e.Op )
                    {
                        case OpType.LessThan:

                            return prev < NextE ( i + 1, null );

                        case OpType.MoreThan:

                            return prev > NextE ( i + 1, null );

                        case OpType.EqualTo:

                            return prev == NextE ( i + 1, null );

                        case OpType.Times:
                            if ( prev is string )
                            {
                                return prev;
                            }
                            return prev * NextE ( i + 1, null );

                        case OpType.Divide:
                            if ( prev is string )
                            {
                                return prev;
                            }
                            return prev / NextE ( i + 1, null );
                            break;

                        case OpType.Plus:
                            return prev + NextE ( i + 1, null );
                            break;

                        case OpType.Minus:
                            return prev - NextE ( i + 1, null );
                    }
                    break;

                case ExprType.VarValue:

                    if(e.VarName == ".")
                    {
                        return NextE(i + 1, prev);
                    }

                    if (prev is StructModule)
                    {
                        val = prev.FindVar (e.VarName).Value;
                    }
                    else
                    {
                        val = ManagedHost.CurrentScope.FindVar(e.VarName, true).Value;
                    }
                    break;

                case ExprType.IntValue:

                    val = e.intV;
                    break;

                case ExprType.StringValue:

                    val = e.StringV;

                    break;

                case ExprType.BoolValue:
                    val = e.BoolV;
                    break;
            }
            if(i<Expr.Count-1)
            {
                return  NextE(i + 1, val);
            }
            return val;
        }

    
    }
}