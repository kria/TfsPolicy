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
using System.Linq;
using System.Text.RegularExpressions;

namespace DevCore.TfsPolicy.Policies
{
    /// <summary>
    /// Enforce branch naming for new branches
    /// </summary>
    public class BranchNamingPolicy : PushPolicy
    {
        private const string BranchPattern = @"^[a-z0-9_./-]+$";

        protected override PolicyEvaluationResult EvaluteInternal(IVssRequestContext requestContext, PushNotification push)
        {
            var newBranches = push.RefUpdateResults.Where(r => r.OldObjectId == Sha1Id.Empty && r.Name.StartsWith(Constants.BranchPrefix));
            var newBranchNames = newBranches.Select(r => r.Name.Substring(Constants.BranchPrefix.Length));

            var badBranchNames = newBranchNames.Where(name => !Regex.IsMatch(name, BranchPattern)
                || Regex.IsMatch(name, "^feat[/-]")
                || Regex.IsMatch(name, "^feature[^/]")
                || name == "dev"
                || name == "development"
                || name == "feature"
                );

            if (badBranchNames.Any())
            {
                Logger.Log("pushNotification:", push);
                var statusMessage = string.Format("Branch naming violation [{0}]. See {1}/guidelines/scm/git-conventions/#branch-naming.",
                    string.Join(", ", badBranchNames), Settings.DocsBaseUrl);
                Logger.Log(statusMessage);
                return new PolicyEvaluationResult(false, push.Pusher, statusMessage);
            }

            return new PolicyEvaluationResult(true);
        }
    }
}
