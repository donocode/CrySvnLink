using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CrySvnLink.Data
{
    public class SymbolicLink
    {
        public String Source { get; set; }
        public String Target { get; set; }

        public void AddFolder(String path, LinkSide linkSide)
        {
            switch (linkSide)
            {
                case LinkSide.Source:
                    Path.IsPathRooted(Source);
                    Source = Path.Combine(path, Source);
                    break;
                case LinkSide.Target:
                    Path.IsPathRooted(Target);
                    Target = Path.Combine(path, Target);
                    break;
            }
        }

        public override bool Equals(object obj)
        {
            SymbolicLink other = obj as SymbolicLink;
            if (other != null)
            {
                if (other.Source != this.Source)
                {
                    return false;
                }
                if (other.Target != this.Target)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            return String.Format("Link - Source({0}), Target({1})");
        }
    }

    public enum LinkSide
    {
        Source = 0,
        Target = 1
    }

    public enum LinkType
    {
        File = 0,
        Folder = 1
    }
}
