using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLibrary.EventArgs
{
    public class OnValidationErrorEventArgs
    {
        public string validationMessage;

        public OnValidationErrorEventArgs(string validationMessage)
        {
            this.validationMessage = validationMessage;
        }
    }
}
