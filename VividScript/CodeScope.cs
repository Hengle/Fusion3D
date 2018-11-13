﻿using VividScript.VStructs;
using System.Collections.Generic;
namespace VividScript
{
    public class CodeScope
    {
        public CodeScope OutterScope = null;


        public string ScopeID = "";

        public CodeScope(string id)
        {

            ScopeID = id;
        
        }


        public List<VSVar> LocalVars = new List<VSVar>();

        public List<FuncLink> LocalFuncs = new List<FuncLink>();

        public virtual void RegisterFunc(FuncLink func)
        {
            LocalFuncs.Add(func);
        }

        public virtual void RegisterVar(VSVar var)
        {
            LocalVars.Add(var);
        }

        public virtual FuncLink FindFunc(string name, bool searchOutter)
        {
            foreach (FuncLink func in LocalFuncs)
            {
                if (func.Name == name)
                {
                    return func;
                }
            }
            if (searchOutter)
            {
                if (OutterScope != null)
                {
                    return OutterScope.FindFunc(name, true);
                }
            }
            // VME.Main.Error ( "Could not find func called:" + name, "ScopeError" );
            return null;
        }

        public virtual VSVar FindVar(string name, bool searchOutter)
        {
            foreach (VSVar v in LocalVars)
            {
                if (v.Name == name)
                {
                    return v;
                }
            }
            if (searchOutter)
            {
                if (OutterScope != null && OutterScope != this)
                {
                    return OutterScope.FindVar(name, true);
                }
            }
            //VME.Main.Error ( "Could not find variable called:" + name, "ScopeError" );
            return null;
        }

    }
}