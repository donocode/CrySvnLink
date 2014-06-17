using CrySvnLink.Messaging;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrySvnLink.Helpers
{
    public class OutputHelper
    {
        private static StringBuilder _outputBuilder = new StringBuilder();

        public static void AppendLine(String text)
        {
            _outputBuilder.AppendLine(text);
            RaiseUpdate();
        }

        public static void Clear()
        {
            _outputBuilder.Clear();
            RaiseUpdate();
        }

        private static void RaiseUpdate()
        {
            Messenger.Default.Send<OutputUpdatedMessage>(new OutputUpdatedMessage(_outputBuilder.ToString()));
        }
    }
}
