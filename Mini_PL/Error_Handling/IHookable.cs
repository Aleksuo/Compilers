﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_PL.Error_Handling.Messages;

namespace Mini_PL.Error_Handling
{
    public interface IHookable
    {
        ErrorHook hook { get; set; }
    }

    public static class ErrorHookExtensions
    {
        public static void HookTo(this IHookable hookable, ErrorManager em)
        {
            hookable.hook.HookTo(em);
        }
        
        public static void Unhook(this IHookable hookable)
        {
            hookable.hook.Unhook();
        }

        public static void ThrowErrorMessage(this IHookable hookable, IErrorMessage message)
        {
            hookable.hook.ThrowErrorMessage(message);
        }    
    }
}
