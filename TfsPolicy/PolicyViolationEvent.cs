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
using System;

namespace DevCore.TfsPolicy
{
    public class PolicyViolationEvent
    {
        public PolicyViolationEvent(Type policyType, object originalEvent, IdentityDescriptor identity, string message)
        {
            PolicyType = policyType;
            OriginalEvent = originalEvent;
            Identity = identity;
            Message = message;
        }
        public Type PolicyType { get; }

        public object OriginalEvent { get; }

        public IdentityDescriptor Identity { get; }

        public string Message { get; }
    }
}
