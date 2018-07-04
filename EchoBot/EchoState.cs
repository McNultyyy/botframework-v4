using System.Collections.Generic;

namespace EchoBot
{
    /// <summary>
    /// Class for storing conversation state. 
    /// </summary>
    public class EchoState : Dictionary<string, object>
    {
        public int? PaymentAmount { get; set; }
        public int? CardPin { get; set; }
        public string RecipientName { get; set; }
    }
}
