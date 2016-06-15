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

using Microsoft.TeamFoundation.Common;
using Microsoft.TeamFoundation.Framework.Server;
using System;

namespace DevCore.TfsPolicy
{
    public abstract class PolicyBase<T> : PolicyBase
    {
        public override Type[] SubscribedTypes()
        {
            return new[] { typeof(T) };
        }

        protected override PolicyEvaluationResult Evalute(IVssRequestContext requestContext, object notificationEventArgs)
        {
            return Evalute(requestContext, (T)notificationEventArgs);
        }

        protected abstract PolicyEvaluationResult Evalute(IVssRequestContext requestContext, T notificationEventArgs);
    }

    public abstract class PolicyBase : ISubscriber
    {
        public string Name => "TfsPolicy";

        public SubscriberPriority Priority => SubscriberPriority.Normal;

        public abstract Type[] SubscribedTypes();

        protected abstract PolicyEvaluationResult Evalute(IVssRequestContext requestContext, object notificationEventArgs);

        public EventNotificationStatus ProcessEvent(IVssRequestContext requestContext, NotificationType notificationType, object notificationEventArgs,
            out int statusCode, out string statusMessage, out ExceptionPropertyCollection properties)
        {
            statusCode = 0;
            statusMessage = string.Empty;
            properties = null;

            if (notificationType == NotificationType.DecisionPoint)
            {
                var result = Evalute(requestContext, notificationEventArgs);
                if (!result.IsValid)
                {
                    statusMessage = result.Message;
                    ITeamFoundationEventService eventService = requestContext.GetService<ITeamFoundationEventService>();
                    eventService.PublishNotification(requestContext, new PolicyViolationEvent(this.GetType(), notificationEventArgs, result.Identity, result.Message));

                    return EventNotificationStatus.ActionDenied;
                }
            }

            return EventNotificationStatus.ActionPermitted;

        }
    }
}
