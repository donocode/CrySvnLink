using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrySvnLink.Messaging
{
    public class OutputUpdatedMessage : MessageBase
    {
        public String NewOutput { get; set; }

        public OutputUpdatedMessage(String newOutput)
        {
            this.NewOutput = newOutput;
        }
    }
}
