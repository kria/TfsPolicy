/*
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

using Microsoft.TeamFoundation.Git.Server;
using System;
using System.Linq;

namespace DevCore.TfsPolicy
{
    public static class Utils
    {
        public static string ToHexString(this byte[] buffer)
        {
            return BitConverter.ToString(buffer).Replace("-", "").ToLower();
        }

        public static string ToHexString(this byte[] buffer, int length)
        {
            return buffer.ToHexString().Substring(0, length);
        }

        public static string ToShortHexString(this byte[] buffer)
        {
            return buffer.ToHexString().Substring(0, 7);
        }

        public static string ToHexString(this Sha1Id id)
        {
            return id.ToByteArray().ToHexString();
        }

        public static string ToHexString(this Sha1Id id, int length)
        {
            return id.ToByteArray().ToHexString(length);
        }

        public static bool IsZero(this byte[] buffer)
        {
            return buffer.All(b => b == 0);
        }

        public static string Dump(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
        }
    }
}
