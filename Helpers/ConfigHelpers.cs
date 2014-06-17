using CrySvnLink.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace CrySvnLink.Helpers
{
    public class ConfigHelpers
    {
        public static SymbolicLink LoadLink(XElement linkNode)
        {
            SymbolicLink link = new SymbolicLink();

            String source = null;
            String target = null;

            if (linkNode.Element("source") != null)
            {
                source = linkNode.Element("source").Value;
            }

            if (linkNode.Element("target") != null)
            {
                target = linkNode.Element("target").Value;
            }

            if (String.IsNullOrWhiteSpace(source))
            {
                OutputHelper.AppendLine("Error: Link source does not exist or is blank. Skipping...");
                return null;
            }

            if (String.IsNullOrWhiteSpace(target))
            {
                OutputHelper.AppendLine("Error: Link target does not exist or is blank. Skipping...");
                return null;
            }

            //check if links already exist

            OutputHelper.AppendLine(String.Format("New link parsed: \n\t Source: {0} \n\t Target: {1}", source, target));
            
            link.Source = source;
            link.Target = target;
            return link;
        }
    }
}
