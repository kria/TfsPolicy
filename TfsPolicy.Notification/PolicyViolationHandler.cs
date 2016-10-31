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

using DevCore.TfsNotificationRelay.EventHandlers;
using DevCore.TfsNotificationRelay.Notifications;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Framework.Server;
using Microsoft.TeamFoundation.Git.Server;
using Microsoft.TeamFoundation.Integration.Server;
using Microsoft.TeamFoundation.Server.Core;
using System.Collections.Generic;

namespace DevCore.TfsPolicy.Notification
{
    public class PolicyViolationHandler : BaseHandler<PolicyViolationEvent>
    {
        protected override IEnumerable<INotification> CreateNotifications(IVssRequestContext requestContext, PolicyViolationEvent policyViolation, int maxLines)
        {
            var commonService = requestContext.GetService<CommonStructureService>();
            var identityService = requestContext.GetService<ITeamFoundationIdentityService>();
            var identity = identityService.ReadIdentity(requestContext, IdentitySearchFactor.Identifier, policyViolation.Identity.Identifier);
            
            var notification = new PolicyViolationNotification()
            {
                TeamProjectCollection = requestContext.ServiceHost.Name,
                ProjectName = string.Empty,
                PolicyType = policyViolation.PolicyType,
                Message = policyViolation.Message,
                UniqueName = identity.UniqueName,
                DisplayName = identity.DisplayName
            };

            var push = policyViolation.OriginalEvent as PushNotification;
            if (push != null)
                notification.ProjectName = commonService.GetProject(requestContext, push.TeamProjectUri).Name;


            yield return notification;
        }
    }
}
