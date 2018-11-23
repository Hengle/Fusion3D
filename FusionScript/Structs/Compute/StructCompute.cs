﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace FusionScript.Structs.Compute
{
    public class StructCompute : Struct
    {

        public string ComputeName = "";
        public List<Compute.StructComputeStruct> Unique = new List<StructComputeStruct>();
        public List<Compute.StructComputeStruct> Inputs = new List<StructComputeStruct>();
        public List<Compute.StructComputeStruct> Outputs = new List<StructComputeStruct>();
        public List<ComputeVar> LocalVars = new List<ComputeVar>();
        public List<StructComputeFunc> Funcs = new List<StructComputeFunc>();
        public string CodeName = "";
        public static int cl_num = 0;
        public void GenCode()
        {
            cl_num++;
            CodeName = "CLGen/CLOut" + cl_num + ".cl";

            TextWriter tw = File.CreateText(CodeName);

            foreach (var s in Unique)
            {

                string head = "typedef struct tag_" + s.StructName + "{";
                tw.WriteLine(head);

                foreach(var v in s.Vars)
                {

                    string var_l = "";
                    switch (v.Type)
                    {
                        case ComputeVarType.Vec3:
                            var_l = "   float3 " + v.Name + ";";
                            break;
                    }

                    tw.WriteLine(var_l);

                }

                string foot = "}" + s.StructName + ";";
                tw.WriteLine(foot);
                tw.WriteLine(" ");
            }
            tw.WriteLine(" ");

            foreach(var fun in Funcs)
            {

                string f_b = "";
                switch (fun.ReturnType)
                {
                    case ComputeVarType.Void:
                        f_b = "void ";
                        break;
                    case ComputeVarType.Vec3:
                        f_b = "float3 ";
                        break;

                }
                bool first2 = true;
                f_b += fun.FuncName + "(";
                foreach(var vv in fun.InVars)
                {
                    if (!first2)
                    {
                        f_b += " , ";
                    }
                    first2 = false;
                    switch (vv.Type)
                    {
                        case ComputeVarType.Int:
                            f_b += " int " + vv.Name;
                            break;
                        case ComputeVarType.Vec3:
                            f_b += " float3 " + vv.Name;
                            break;
                    }
                }
                f_b += " )";

                tw.WriteLine(f_b);
                tw.WriteLine("{");
                tw.WriteLine("}");

            }
            tw.WriteLine(" ");

            string kern_head = "__kernel void " + ComputeName + "(";

            bool first = true;
            foreach(var iv in Inputs)
            {

                kern_head += "__global " + iv.StructName + " *" + iv.LocalName;
                if (!first)
                {
                    kern_head += " , ";
                }
                first = false;


            }

            kern_head += ")";

            tw.WriteLine(kern_head);
            tw.WriteLine("{");
            tw.WriteLine("  int index = get_global_id(0);");
            tw.WriteLine("}");


            tw.Flush();
            tw.Close();

        }

    }
}
