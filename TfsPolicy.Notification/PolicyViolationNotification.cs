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

using DevCore.TfsNotificationRelay;
using DevCore.TfsNotificationRelay.Configuration;
using DevCore.TfsNotificationRelay.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevCore.TfsPolicy.Notification
{
    public class PolicyViolationNotification : BaseNotification
    {
        public string ProjectName { get; set; }
        public string UniqueName { get; set; }
        public string DisplayName { get; set; }
        public Type PolicyType { get; set; }
        public string Message { get; set; }

        public override EventRuleElement GetRuleMatch(string collection, IEnumerable<EventRuleElement> eventRules)
        {
            var rule = eventRules.FirstOrDefault(r => r.Events.HasFlag(TfsEvents.PolicyViolation)
                && collection.IsMatchOrNoPattern(r.TeamProjectCollection)
                && ProjectName.IsMatchOrNoPattern(r.TeamProject));

            return rule;
        }

        public override IList<string> ToMessage(BotElement bot, TextElement text, Func<string, string> transform)
        {
            return new[] { $"Policy violation ({PolicyType}) by {DisplayName}: {Message}" };
        }
    }
}
