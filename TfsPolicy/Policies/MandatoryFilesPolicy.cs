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
using Microsoft.TeamFoundation.SourceControl.WebApi;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DevCore.TfsPolicy.Policies
{
    /// <summary>
    /// Check that any new master branch (i.e. newly created repo) has a .gitattributes and .gitignore file in the root
    /// </summary>
    public class MandatoryFilesPolicy : PushPolicy
    {
        protected override PolicyEvaluationResult EvaluteInternal(IVssRequestContext requestContext, PushNotification push)
        {
            var repositoryService = requestContext.GetService<ITeamFoundationGitRepositoryService>();

            var newBranches = push.RefUpdateResults.Where(r => r.OldObjectId == Sha1Id.Empty && r.Name.StartsWith(Constants.BranchPrefix));
            var branch = newBranches.SingleOrDefault(b => b.Name == Constants.BranchPrefix + "master");
            if (branch != null)
            {
                using (ITfsGitRepository repository = repositoryService.FindRepositoryById(requestContext, push.RepositoryId))
                {
                    TfsGitCommit commit = (TfsGitCommit)repository.LookupObject(branch.NewObjectId);
                    if (commit != null)
                    {
                        var tree = commit.GetTree(requestContext);
                        var treeEntries = tree.GetTreeEntries(requestContext);
                        if (treeEntries.Any())
                        {
                            bool includesGitignore = false;
                            bool includesGitattributes = false;
                            foreach (var entry in treeEntries)
                            {
                                if (entry.ObjectType == GitObjectType.Blob && entry.RelativePath.Equals(".gitignore", StringComparison.OrdinalIgnoreCase))
                                    includesGitignore = true;

                                if (entry.ObjectType == GitObjectType.Blob && entry.RelativePath.Equals(".gitattributes", StringComparison.OrdinalIgnoreCase))
                                {
                                    using (var reader = new StreamReader(entry.Object.GetContent(requestContext)))
                                    {
                                        var gitattributesContents = reader.ReadToEnd();
                                        // Make sure .gitattributes file has a '* text=auto' line for eol normalization
                                        if (!Regex.IsMatch(gitattributesContents, @"^\*\s+text=auto\s*$", RegexOptions.Multiline)) {
                                            Logger.Log("pushNotification:", push);
                                            var statusMessage = $".gitattributes is missing '* text=auto'. See {Settings.DocsBaseUrl}/guidelines/scm/git-conventions/#mandatory-files.";
                                            Logger.Log(statusMessage);
                                            return new PolicyEvaluationResult(false, push.Pusher, statusMessage);
                                        }

                                        includesGitattributes = true;
                                    }
                                }
                                    
                            }

                            if (!includesGitignore || !includesGitattributes)
                            {
                                Logger.Log("pushNotification:", push);
                                var statusMessage = $"Mandatory files missing. See {Settings.DocsBaseUrl}/guidelines/scm/git-conventions/#mandatory-files.";
                                Logger.Log(statusMessage);
                                return new PolicyEvaluationResult(false, push.Pusher, statusMessage);
                            }
                        }
                        else
                        {
                            Logger.Log("Commit without tree entries: " + branch.NewObjectId);
                        }
                    }
                    else
                    {
                        Logger.Log("Unable to find commit " + branch.NewObjectId);
                    }
                }
            }

            return new PolicyEvaluationResult(true);
        }
    }
}
