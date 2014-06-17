using CrySvnLink.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using WPFCustomMessageBox;

namespace CrySvnLink.Helpers
{
    public class MessageBoxHelper
    {
        private const String _delete = "Delete";
        private const String _skip = "Skip";
        private const String _cancel = "Cancel";

        public static MkLinkMessageResult SourceFolderExists(SymbolicLink link)
        {
            String message = String.Format("{0} \nalready exists, you may have already created this link. \n\n What do you want to do?", link.Source);
            String caption = "Source Folder Exists";

            MessageBoxResult result = CustomMessageBox.ShowYesNoCancel(message, caption, _delete, _skip, _cancel);

            switch (result)
            {
                case MessageBoxResult.Cancel:
                    OutputHelper.AppendLine("Operation cancelled by user");
                    return MkLinkMessageResult.Cancel;
                case MessageBoxResult.No:
                    OutputHelper.AppendLine("Skipping link...");
                    return MkLinkMessageResult.Skip;
                case MessageBoxResult.Yes:
                    OutputHelper.AppendLine(String.Format("Deleting Folder {0}", link.Source));
                    return MkLinkMessageResult.Delete;
            }

            return MkLinkMessageResult.Cancel;
        }

        public static MkLinkMessageResult SourceFileExists(SymbolicLink link)
        {
            String message = String.Format("{0} \nalready exists, you may have already created this link. \n\n What do you want to do?", link.Source);
            String caption = "Source File Exists";

            MessageBoxResult result = CustomMessageBox.ShowYesNoCancel(message, caption, _delete, _skip, _cancel);

            switch (result)
            {
                case MessageBoxResult.Cancel:
                    OutputHelper.AppendLine("Operation cancelled by user");
                    return MkLinkMessageResult.Cancel;
                case MessageBoxResult.No:
                    OutputHelper.AppendLine("Skipping link...");
                    return MkLinkMessageResult.Skip;
                case MessageBoxResult.Yes:
                    OutputHelper.AppendLine(String.Format("Deleting File {0}", link.Source));
                    return MkLinkMessageResult.Delete;
            }

            return MkLinkMessageResult.Cancel;
        }

    }

    public enum MkLinkMessageResult
    {
        Cancel,
        Delete,
        Skip
    }
}
