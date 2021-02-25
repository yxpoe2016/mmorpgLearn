﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;

namespace Common.Data
{
    public enum NpcType
    {
        None = 0,
        Functional = 1,
        Task = 2,
    }

    public enum NpcFunction
    {
        None =0,
        InvokeShop =1,
        InvokeInsrance = 2,
    }

    public class NpcDefine
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Param { get; set;}
        public string Description { get; set; }
        public NpcType Type { get; set; }
        public NpcFunction Function { get; set; }
    }
}