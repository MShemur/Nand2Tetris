using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualMachine
{
    public enum CommandType
    {
        Arithmetic,
        Push,
        Pop,
        Label,
        GoTo,
        If,
        Function,
        Return,
        Call
    }
}
