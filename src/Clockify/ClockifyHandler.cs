using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bot.Clockify.Client;
using Bot.Clockify.Fill;
using Bot.Clockify.Models;
using Bot.Clockify.Reports;
using Bot.Common.Recognizer;
using Bot.Data;
using Bot.States;
using Bot.Supports;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace Bot.Clockify
{
    public class ClockifyHandler : IBotHandler
    {
        private readonly EntryFillDialog _fillDialog;
        private readonly ReportDialog _reportDialog;
        private readonly StopReminderDialog _stopReminderDialog;
        private readonly ClockifySetupDialog _clockifySetupDialog;
        private readonly IClockifyService _clockifyService;
        private readonly DialogSet _dialogSet;
        private readonly IStatePropertyAccessor<DialogState> _dialogState;
        private readonly ITokenRepository _tokenRepository;

        public ClockifyHandler(EntryFillDialog fillDialog, ReportDialog reportDialog,
            StopReminderDialog stopReminderDialog, IClockifyService clockifyService,
            ConversationState conversationState, ClockifySetupDialog clockifySetupDialog,
            ITokenRepository tokenRepository)
        {
            _dialogState = conversationState.CreateProperty<DialogState>("ClockifyDialogState");
            _fillDialog = fillDialog;
            _reportDialog = reportDialog;
            _stopReminderDialog = stopReminderDialog;
            _clockifyService = clockifyService;
            _clockifySetupDialog = clockifySetupDialog;
            _tokenRepository = tokenRepository;
            _dialogSet = new DialogSet(_dialogState)
                .Add(_fillDialog)
                .Add(_stopReminderDialog)
                .Add(_reportDialog)
                .Add(_clockifySetupDialog);
        }

        public async Task<bool> Handle(ITurnContext turnContext, CancellationToken cancellationToken,
            UserProfile userProfile, TimeSurveyBotLuis? luisResult = null)
        {
            if (luisResult == null)
            {
                return false;
            }

            var dialogContext = await _dialogSet.CreateContextAsync(turnContext, cancellationToken);

            if (await RunClockifySetupIfNeeded(turnContext, cancellationToken, userProfile)) return true;

            try
            {
                switch (luisResult.TopIntentWithMinScore())
                {
                    case TimeSurveyBotLuis.Intent.Report:
                        await dialogContext.BeginDialogAsync(_reportDialog.Id, luisResult, cancellationToken);
                        return true;
                    case TimeSurveyBotLuis.Intent.Fill:
                        await dialogContext.BeginDialogAsync(_fillDialog.Id, luisResult, cancellationToken);
                        return true;
                    case TimeSurveyBotLuis.Intent.FillAsYesterday:
                        return false;
                    case TimeSurveyBotLuis.Intent.Utilities_Stop:
                        await dialogContext.BeginDialogAsync(_stopReminderDialog.Id,
                            cancellationToken: cancellationToken);
                        return true;
                    default:
                        return false;
                }
            }
            catch (UnauthorizedAccessException)
            {
                await dialogContext.BeginDialogAsync(_clockifySetupDialog.Id, _dialogState, cancellationToken);
                return true;
            }
        }

        public DialogSet GetDialogSet() => _dialogSet;

        private async Task<bool> RunClockifySetupIfNeeded(ITurnContext turnContext, CancellationToken cancellationToken,
            UserProfile userProfile)
        {
            if (userProfile.ClockifyTokenId != null) return false;
            await _clockifySetupDialog.RunAsync(turnContext, _dialogState, cancellationToken);
            return true;
        }
    }
}