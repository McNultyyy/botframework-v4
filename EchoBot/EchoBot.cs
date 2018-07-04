using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using EchoBot.Dialogs;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Prompts;
using Microsoft.Bot.Schema;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using Microsoft.Recognizers.Text;
using TextPrompt = Microsoft.Bot.Builder.Dialogs.TextPrompt;

namespace EchoBot
{
    public static class PromptStep
    {
        public const string Welcome = "welcome";
        public const string NamePrompt = "namePrompt";
        public const string AgePrompt = "agePrompt";
    }

    public class EchoBot : IBot
    {
        private readonly DialogSet dialogs;

        private async Task RecipientNameValidator(ITurnContext context, TextResult result)
        {
            if (result.Value.Length <= 2)
            {
                result.Status = PromptStatus.NotRecognized;
                await context.SendActivity("Your name should be at least 2 characters long.");
            }
        }

        private async Task CardPinValidator(ITurnContext context, TextResult result)
        {
            if (result.Value.Length != 4)
            {
                result.Status = PromptStatus.NotRecognized;
                await context.SendActivity("A PIN must be 4 digits.");
            }
        }

        private async Task AskNameStep(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            var state = dialogContext.Context.GetConversationState<EchoState>();
            if (state.RecipientName != null)
                await dialogContext.Continue();

            await dialogContext.Prompt(PromptStep.NamePrompt, "What is your name?");

            var t = "";
        }

        private async Task AskAgeStep(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            var state = dialogContext.Context.GetConversationState<EchoState>();
            state.RecipientName = (result as TextResult).Value;
            await dialogContext.Prompt(PromptStep.AgePrompt, "What is your age?");
        }

        private async Task GatherInfoStep(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            var state = dialogContext.Context.GetConversationState<EchoState>();
            state.CardPin = (result as NumberResult<int>).Value;
            await dialogContext.Context.SendActivity($"Your name is {state.RecipientName} and your age is {state.CardPin}");
            await dialogContext.End();
        }

        public EchoBot()
        {
            dialogs = new DialogSet();

            // Create prompt for name with string length validation
            dialogs.Add(PromptStep.NamePrompt, new TextPrompt(RecipientNameValidator));
            // Create prompt for age with number value validation
            dialogs.Add(PromptStep.AgePrompt, new TextPrompt(CardPinValidator));
            // Add a dialog that uses both prompts to gather information from the user
            dialogs.Add(PromptStep.Welcome,
                new WaterfallStep[] { AskNameStep, AskAgeStep, GatherInfoStep });
        }

        public async Task OnTurn(ITurnContext context)
        {
            var state = context.GetConversationState<EchoState>();
            var dialogCtx = dialogs.CreateContext(context, state);
            switch (context.Activity.Type)
            {
                case ActivityTypes.Message:
                    await dialogCtx.Continue();
                    if (!context.Responded)
                    {
                        await dialogCtx.Begin(PromptStep.Welcome);
                    }

                    break;
            }
        }

    }
}



