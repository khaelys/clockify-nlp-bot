﻿using System.Threading;
using System.Threading.Tasks;
using Bot.Clockify;
using Bot.Common;
using Bot.States;
using Luis;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace Bot.Supports
{
    public class UtilityHandler : IBotHandler
    {
        private readonly LuisRecognizerProxy _luisRecognizer;
        private readonly DialogSet _dialogSet;
        private readonly ICommonMessageSource _messageSource;

        public UtilityHandler(ConversationState conversationState, LuisRecognizerProxy luisRecognizer,
            ICommonMessageSource messageSource)
        {
            IStatePropertyAccessor<DialogState> dialogState =
                conversationState.CreateProperty<DialogState>("UtilityDialogState");
            _luisRecognizer = luisRecognizer;
            _messageSource = messageSource;
            _dialogSet = new DialogSet(dialogState);
        }

        public async Task<bool> Handle(ITurnContext turnContext, CancellationToken cancellationToken,
            UserProfile userProfile)
        {
            if (userProfile.ClockifyToken == null && userProfile.ClockifyTokenId == null)
            {
                await ExplainBot(turnContext, cancellationToken);
            }

            var (topIntent, _) =
                await _luisRecognizer.RecognizeAsyncIntent(turnContext, cancellationToken);

            switch (topIntent)
            {
                case TimeSurveyBotLuis.Intent.Thanks:
                {
                    string message = _messageSource.ThanksAnswer;
                    await turnContext.SendActivityAsync(MessageFactory.Text(message), cancellationToken);
                    return true;
                }
                case TimeSurveyBotLuis.Intent.Insult:
                {
                    string message = _messageSource.InsultAnswer;
                    await turnContext.SendActivityAsync(MessageFactory.Text(message), cancellationToken);
                    return true;
                }
                case TimeSurveyBotLuis.Intent.Utilities_Help:
                {
                    await ExplainBot(turnContext, cancellationToken);
                    return true;
                }
                default:
                    return false;
            }
        }

        public DialogSet GetDialogSet() => _dialogSet;

        private async Task ExplainBot(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync(
                MessageFactory.Text(string.Format(_messageSource.HelpIntro, "\n", turnContext.Activity.ChannelId)),
                cancellationToken);

            await turnContext.SendActivityAsync(
                MessageFactory.Text(string.Format(_messageSource.HelpDescription, "\n")), cancellationToken);

            await turnContext.SendActivityAsync(MessageFactory.Text(_messageSource.HelpSecurityInfo), cancellationToken);

            await turnContext.SendActivityAsync(MessageFactory.Text(_messageSource.HelpLanguage), cancellationToken);
        }
    }
}