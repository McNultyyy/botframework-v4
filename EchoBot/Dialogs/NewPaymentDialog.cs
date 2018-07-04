using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace EchoBot.Dialogs
{
    public class NewPaymentDialog : DialogContainer
    {
        public const string Id = nameof(NewPaymentDialog);

        public NewPaymentDialog() : base(Id)
        {
            Dialogs.Add(Id, new WaterfallStep[]
            {
            });
        }

    }
}