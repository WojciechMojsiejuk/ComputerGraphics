﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ComputerGraphics
{
    public static class Operation
    {
        public enum Option
        {
            Select,
            Move,
            Resize,
            Rotate,
            Create
        }
        public static Option option;
    }
}
