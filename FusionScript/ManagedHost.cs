﻿using System;
using FusionScript.Structs;
using System.Collections.Generic;
namespace FusionScript
{
    public class ManagedHost
    {
        public CodeScope SystemScope = new CodeScope("SystemScope");
        public System.Collections.Generic.Stack<CodeScope> Scopes = new System.Collections.Generic.Stack<CodeScope>();
        public List<Module> Mods = new List<Module>();

        public void RegisterOSFuncs ( )
        {
            // - Millisecs()

            CFuncLink millisecs = new CFuncLink
            {
                Link = (t) =>
                {
                    return (dynamic)Environment.TickCount;
                }
            };

            // - printF( <Expressions> )

            CFuncLink printF = new CFuncLink
            {
                Link = ( t ) =>
                {
                    Console.WriteLine ( "printF:" + t[0]);

                    return null;
                }
            };

            AddCFunc ( "printf", printF );
        }

        public static ManagedHost Main = null;

        public StructEntry Entry = null;

        public static CodeScope CurrentScope => Main.Scopes.Peek ( );

        public void AddModule(Module mod)
        {
            Mods.Add(mod);
            foreach(var mv in mod.Mod.Modules)
            {
               
            }

        }
        public StructModule FindClass(string name)
        {
            foreach(var bmod in Mods)
            {

                foreach(var mod in bmod.Mod.Modules)
                {
                    if(mod.ModuleName == name)
                    {
                        return mod;
                    }
                }

            }
            return null;
        }
        public StructModule FindMod(string name)
        {
            foreach(var mod in Mods)
            {
               foreach(var imod in mod.Mod.Modules)
                {
                    if(imod.ModuleName == name)
                    {
                        return imod;
                    }
                }
            }
            return null;
        }
        public Var FindVar(string name)
        {

            foreach(var mod in Mods)
            {

                foreach(var imod in mod.Mod.Modules)
                {
                    var cv = imod.FindVar(name);
                    if (cv != null) return cv;
                    
                }

            }
            return null;
        }
        public void AddCFunc ( string name, CFuncLink link )
        {
            FuncLink FuncLink = new FuncLink
            {
                Name = name,
                Link = link,
                Type = FuncType.CSharp,
            };
            SystemScope.RegisterFunc ( FuncLink );
        }

        public ManagedHost ( )
        {
            Main = this;
            RegisterOSFuncs ( );
        }

        public void Error ( string err, string type = "", Struct point = null )
        {
            Console.WriteLine ( "Error:->" + err + "@" + type );
        }

        public void Log ( string msg, string type = "", Struct point = null )
        {
            Console.WriteLine ( "Message:->" + msg + "@" + type );
        }

        public void SetEntry ( Structs.StructEntry entry )
        {
            Entry = entry;
        }

        public CodeScope RunEntry ( )
        {
            StructFunc entry_func = Entry.FindSystemFunc("Entry");
            if ( entry_func == null )
            {
                Error ( "Unable to find entry point." );
            }
            PushScope ( SystemScope );
            return ExecuteFunc ( entry_func );
            PopScope ( );
        }

        public static void PopScope ( )
        {
            Main.Scopes.Pop ( );
        }

        public static void PushScope ( CodeScope scope )
        {
            if ( Main.Scopes.Count != 0 )
            {
                scope.OutterScope = Main.Scopes.Peek ( );
            }
            else
            {
                scope.OutterScope = null;
            }
            Main.Scopes.Push ( scope );
        }
        public CodeScope ExecuteMethod(StructModule cls,string func,dynamic[] pars)
        {

            foreach(var f in cls.Methods)
            {
                if(f.FuncName == func)
                {
                    int ii = 0;
                    var ns = new CodeScope("func_pars");
                    foreach(var p in pars)
                    {
                        var rp = f.Pars.Pars[ii];
                        ns.RegisterVar(rp);
                        rp.Value = p;
                        ii++;
                    }
                    PushScope(cls.InstanceScope);
                    PushScope(ns);
                    f.Code.Exec();
                    PopScope();
                    PopScope();
                }
            }
            return null;
        }
        public CodeScope ExecuteStaticFunc(string name)
        {
            foreach(var mod in Mods)
            {
                foreach(var sf in mod.Mod.SystemFuncs)
                {

                    if (sf.FuncName == name)
                    {
                        PushScope(SystemScope);
                        PushScope(sf.LocalScope);
                        var rv = ExecuteFunc(sf);
                        PopScope();

                        PopScope();
                        return rv;
                    }

                }
                foreach(var imod in mod.Mod.Modules)
                {

                    foreach(var sfunc in imod.StaticFuncs)
                    {

                        if(sfunc.FuncName == name)
                        {

                            PushScope(imod.StaticScope);
                            var rv= ExecuteFunc(sfunc);
                            PopScope();
                            return rv;


                        }

                    }

                }

            }
            return null;

        }

        public CodeScope ExecuteFunc ( StructFunc func )
        {
            Log ( "Running Func:" + func.Name );

   

         

            func.Code.Exec ( );

 
            return func.LocalScope;
        }
    }
}