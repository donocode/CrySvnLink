using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CrySvnLink.ViewModel
{
    public class AboutViewModel : ViewModelBase
    {
        #region Fields

        private String _name = "CrySVNLink";
        private String _description = "A utility tool created to aid TimeSplitters Rewind development, open sourced to aid everyone else";
        private String _frameworks = "Frameworks used: \nMvvmLight \nWPFCustomMessageBox";
        private String _tsRewindUrl = "http://www.tsrewind.com";

        private String _assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        #endregion

        #region Properties

        public String Name
        {
            get { return _name; }
        }

        public String Description
        {
            get { return _description; }
        }

        public String Frameworks
        {
            get { return _frameworks; }
        }

        public String TsRewindUrl
        {
            get { return _tsRewindUrl; }
        }

        public String AssemblyVersion
        {
            get { return _assemblyVersion; }
        }

        #endregion


    }
}
