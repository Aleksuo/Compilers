﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_PL
{
    public enum TokenType
    {
        //Operators
        PLUS, MINUS, MULT, DIV, AND, OR, NOT, EQUALS, LESSTHAN,
        //Value types
        INTEGER, STRING, BOOLEAN,
        //Keywords
        FOR, IN, DO, VAR, END, READ,
        PRINT, ASSERT, TYPE,
        //Other
        EOF, ERROR, LEFTPAREN, RIGHTPAREN,
        COLON, SEMICOLON, ID, ASSIGN, RANGE        
    }
}