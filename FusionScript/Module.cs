﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FusionScript.Structs;
namespace FusionScript
{
    public class Module
    {

        public StructEntry Mod;
        public ManagedHost.CModule CMod;
        public Module(StructEntry entry)
        {

            Mod = entry;

        }
        public Module()
        {

        }

    }
}
