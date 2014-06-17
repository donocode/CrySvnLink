using CrySvnLink.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CrySvnLink.Helpers
{
    public class MkLinkHelper
    {
        [DllImport("kernel32.dll")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.I1)]
        static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, LinkType dwFlags);

        public static bool CreateLink(SymbolicLink link)
        {
            LinkType linkType = FileHelpers.IsPathDirectory(link.Target) ? LinkType.Folder : LinkType.File;

            bool success = CreateSymbolicLink(link.Source, link.Target, linkType);

            return success;
        }


    }
}
