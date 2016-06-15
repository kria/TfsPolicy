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

using Microsoft.VisualStudio.Services.Identity;

namespace DevCore.TfsPolicy
{
    public class PolicyEvaluationResult
    {
        public PolicyEvaluationResult(bool valid, IdentityDescriptor identity, string msg)
        {
            IsValid = valid;
            Identity = identity;
            Message = msg;
        }
        public PolicyEvaluationResult(bool valid) : this(valid, null, null)
        {
        }

        public bool IsValid { get; }

        public IdentityDescriptor Identity { get; }

        public string Message { get; }
    }
}
