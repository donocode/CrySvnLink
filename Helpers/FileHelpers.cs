using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CrySvnLink.Helpers
{
    public class FileHelpers
    {
        public static Boolean IsPathDirectory(string path)
        {
            return !Path.HasExtension(path);
        }
    }
}
