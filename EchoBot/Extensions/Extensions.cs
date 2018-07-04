using Microsoft.Bot.Builder.Dialogs;

namespace EchoBot.Extensions
{
    public static class Extensions
    {
        public static IDialog Add<T>(this DialogSet dialogSet, T dialog) where T : IDialog
        {
            return dialogSet.Add(typeof(T).Name, dialog);
        }
    }
}