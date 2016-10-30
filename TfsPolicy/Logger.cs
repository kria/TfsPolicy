﻿/*
 * TfsPolicy - http://github.com/kria/TfsPolicy
 * 
 * Copyright (C) 2016 Kristian Adrup
 * 
 * This file is part of TfsNotificationRelay.
 * 
 * TfsNotificationRelay is free software: you can redistribute it and/or 
 * modify it under the terms of the GNU General Public License as published 
 * by the Free Software Foundation, either version 3 of the License, or 
 * (at your option) any later version. See included file COPYING for details.
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace DevCore.TfsPolicy
{
    public static class Logger
    {
        public static void Log(IEnumerable<string> lines)
        {
            if (string.IsNullOrEmpty(Settings.Logfile)) return;

            using (StreamWriter sw = File.AppendText(Settings.Logfile))
            {
                foreach (string line in lines)
                {
                    sw.WriteLine("[{0}] {1}", DateTime.Now, line);
                }
            }
        }

        public static void Log(string line)
        {
            Log(new[] { line });
        }

        public static void Log(string format, params object[] args)
        {
            Log(string.Format(format, args));
        }

        public static void Log(Exception ex)
        {
            Log(ex.ToString());
        }

        public static void Log(string arg, object obj)
        {
            Log(arg + ":" + Utils.Dump(obj));
        }
    }
}
