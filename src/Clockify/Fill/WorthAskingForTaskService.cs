﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Clockify.Client;
using Bot.Clockify.Models;
using Bot.Data;
using Bot.States;

namespace Bot.Clockify.Fill
{
    public class WorthAskingForTaskService
    {
        private readonly IClockifyService _clockifyService;
        private readonly ITokenRepository _tokenRepository;

        public WorthAskingForTaskService(IClockifyService clockifyService, ITokenRepository tokenRepository)
        {
            _clockifyService = clockifyService;
            _tokenRepository = tokenRepository;
        }

        public async Task<bool> IsWorthAskingForTask(ProjectDo project, UserProfile userProfile)
        {
            var tokenData = await _tokenRepository.ReadAsync(userProfile.ClockifyTokenId!);
            string clockifyToken = tokenData.Value;
            string userId = userProfile.UserId ?? throw new ArgumentNullException(nameof(userProfile.UserId));
            var associatedTasks = await _clockifyService.GetTasksAsync(clockifyToken, project.WorkspaceId, project.Id);
            if (!associatedTasks.Any()) return false;
            var end = DateTimeOffset.Now;
            var start = end.AddDays(-90);
            List<HydratedTimeEntryDo> history = await _clockifyService.GetHydratedTimeEntriesAsync(
                clockifyToken,
                project.WorkspaceId,
                userId,
                start,
                end);
            history = history.Where(e => e.Project.Id == project.Id).ToList();
            int totalHistorySize = history.Count;
            int historySizeWithTaskPopulated = history.Count(e => e.Task != null);
            bool thereIsEnoughHistory = totalHistorySize > 5;
            bool tasksAreUsuallySetOnThisProject = historySizeWithTaskPopulated >= 0.3 * totalHistorySize;
            return !thereIsEnoughHistory || tasksAreUsuallySetOnThisProject;
        }
    }
}