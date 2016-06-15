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

using Microsoft.TeamFoundation.Framework.Server;
using Microsoft.TeamFoundation.Git.Server;
using Microsoft.TeamFoundation.Integration.Server;

namespace DevCore.TfsPolicy.Policies
{
    public abstract class PushPolicy : PolicyBase<PushNotification>
    {
        protected override PolicyEvaluationResult Evalute(IVssRequestContext requestContext, PushNotification notificationEventArgs)
        {
            var commonService = requestContext.GetService<ICommonStructureService>();
            var push = (PushNotification)notificationEventArgs;
            var projectName = commonService.GetProject(requestContext, push.TeamProjectUri).Name;
            if (projectName == "TestCompany")
                return new PolicyEvaluationResult(true);

            return EvaluteInternal(requestContext, notificationEventArgs);
        }

        
        protected abstract PolicyEvaluationResult EvaluteInternal(IVssRequestContext requestContext, PushNotification notificationEventArgs);

    }
}
