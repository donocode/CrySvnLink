using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using CrySvnLink.Data;
using CrySvnLink.Messaging;
using CrySvnLink.Helpers;
using System.Collections.Generic;

namespace CrySvnLink.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private String _folderSource;
        private String _folderTarget;
        private String _dllName;
        private String _output;

        private ObservableCollection<SymbolicLink> _links;

        #endregion

        #region Properties

        public String DllName
        {
            get { return _dllName; }
            set
            {
                _dllName = value;
                RaisePropertyChanged<String>(() => DllName);
            }
        }

        public String FolderSource
        {
            get { return _folderSource; }
            set
            {
                _folderSource = value;
                RaisePropertyChanged<String>(() => FolderSource);
            }
        }

        public String FolderTarget
        {
            get { return _folderTarget; }
            set
            {
                _folderTarget = value;
                RaisePropertyChanged<String>(() => FolderTarget);
            }
        }

        public String Output
        {
            get { return _output; }
            set
            {
                _output = value;
                RaisePropertyChanged<String>(() => Output);
            }
        }

        public ObservableCollection<SymbolicLink> Links
        {
            get { return _links; }
            set
            {
                _links = value;
                RaisePropertyChanged<ObservableCollection<SymbolicLink>>(() => Links);
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Links = new ObservableCollection<SymbolicLink>();
            RegisterForMessages();
            ReadConfigFile();
        }

        #region Commands

        #region DllNameHelpCommand

        private RelayCommand<Int32> _dllNameHelpCommand;

        public RelayCommand<Int32> DllNameHelpCommand
        {
            get
            {
                if (_dllNameHelpCommand == null)
                {
                    _dllNameHelpCommand = new RelayCommand<int>(DllNameHelpCommand_Executed);
                }

                return _dllNameHelpCommand;
            }
        }

        private void DllNameHelpCommand_Executed(int buttonId)
        {
            MessageBox.Show("I suggest that this is not CryGameSDK.dll as steam will constantly overwrite this, rather create your own named dll and use that.", "Help", MessageBoxButtons.OK);
        }

        #endregion

        #region BrowseCommand

        private RelayCommand<Int32> _browseCommand;

        public RelayCommand<Int32> BrowseCommand
        {
            get
            {
                if (_browseCommand == null)
                {
                    _browseCommand = new RelayCommand<int>(BrowseCommand_Executed);
                }

                return _browseCommand;
            }
        }

        private void BrowseCommand_Executed(int buttonId)
        {
            BrowseForPath(buttonId);
        }

        #endregion

        #region CreateLinksCommand

        private RelayCommand _createLinksCommand;

        public RelayCommand CreateLinksCommand
        {
            get
            {
                if (_createLinksCommand == null)
                {
                    _createLinksCommand = new RelayCommand(CreateLinksCommand_Executed, CreateLinksCommand_CanExecute);
                }

                return _createLinksCommand;
            }
        }

        private void CreateLinksCommand_Executed()
        {
            CreateLinks();
        }

        private Boolean CreateLinksCommand_CanExecute()
        {
            //check if folder paths are filled
            if (String.IsNullOrWhiteSpace(FolderSource) || String.IsNullOrWhiteSpace(FolderTarget))
            {
                //Output = "Both folder paths must be filled to create links";
                return false;
            }

            //Check source folder is an actual folder
            if (!Directory.Exists(FolderSource))
            {
                //Output = String.Format("Directory - {0} - Does not exist", FolderSource);
                return false;
            }

            //Check target folder is an actual folder
            if (!Directory.Exists(FolderTarget))
            {
                //Output = String.Format("Directory - {0} - Does not exist", FolderTarget);
                return false;
            }

            //Output = "";
            return true;
        }

        #endregion

        #region AboutCommand

        private RelayCommand<Int32> _aboutCommand;

        public RelayCommand<Int32> AboutCommand
        {
            get
            {
                if (_aboutCommand == null)
                {
                    _aboutCommand = new RelayCommand<int>(AboutCommand_Executed);
                }

                return _aboutCommand;
            }
        }

        private void AboutCommand_Executed(int buttonId)
        {
            AboutWindow wnd = new AboutWindow();
            wnd.Owner = App.Current.MainWindow;
            wnd.ShowDialog();
        }

        #endregion

        #endregion

        #region Methods

        #region Private Methods

        private void RegisterForMessages()
        {
            Messenger.Default.Register<OutputUpdatedMessage>(this, (msg) => Output = msg.NewOutput);
        }

        private void BrowseForPath(int buttonId)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            switch (buttonId)
            {
                case 0:
                    fbd.SelectedPath = FolderSource;
                    fbd.Description = "Select root CRYENGINE install folder in your steam directory";
                    break;
                case 1:
                    fbd.SelectedPath = FolderTarget;
                    fbd.Description = "Select the folder in your local repo containing content. Should have a game folder with several folders (Objects, Textures etc)";
                    break;
            }

            fbd.ShowDialog();

            switch (buttonId)
            {
                case 0:
                    FolderSource = fbd.SelectedPath;
                    break;
                case 1:
                    FolderTarget = fbd.SelectedPath;
                    break;
            }
        }

        private void ReadConfigFile()
        {
            Output += "Reading config file\n";

            XDocument doc = XDocument.Load("Resources/config.xml");
            XElement root = doc.Root;

            if (root.Name.LocalName.ToLower() != "crysvnlinkconfig")
            {
                MessageBox.Show("Config file does not contain a CrySvnLinkConfig root node");
                return;
            }

            if (root.Element("gamedll") != null)
            {
                DllName = root.Element("gamedll").Value;
            }

            foreach (var item in (from e in root.Elements("link") select e))
            {
                SymbolicLink link = ConfigHelpers.LoadLink(item);
                if (link != null)
                {
                    Links.Add(link);
                }
            }
        }

        private void CreateLinks()
        {
            //Copy links
            List<SymbolicLink> tempLinks = new List<SymbolicLink>(Links);

            OutputHelper.Clear();

            //Create temp game dll links
            String temp = Path.Combine("bin32", DllName);
            SymbolicLink dllLink32 = new SymbolicLink();
            dllLink32.Source = temp;
            dllLink32.Target = temp;
            temp = Path.Combine("bin64", DllName);
            tempLinks.Insert(0, dllLink32);
            SymbolicLink dllLink64 = new SymbolicLink();
            dllLink64.Source = temp;
            dllLink64.Target = temp;
            tempLinks.Insert(1, dllLink64);

            OutputHelper.AppendLine(String.Format("Beginning link creation: {0} Links", tempLinks.Count));
            int createdCount = 0;
            int total = 0;


            foreach (var link in tempLinks)
            {
                total++;

                link.AddFolder(FolderSource, LinkSide.Source);
                link.AddFolder(FolderTarget, LinkSide.Target);

                OutputHelper.AppendLine(String.Format("Creating link \n From: {0} \n To: {1}", link.Source, link.Target));

                //Check if file/folder exists at source
                if (Directory.Exists(link.Source))
                {
                    MkLinkMessageResult result = MessageBoxHelper.SourceFolderExists(link);

                    if (result == MkLinkMessageResult.Cancel)
                    {
                        OutputHelper.AppendLine(String.Format("{0} links created before cancellation", createdCount));
                        return;
                    }
                    if (result == MkLinkMessageResult.Skip)
                    {
                        continue;
                    }
                    if (result == MkLinkMessageResult.Delete)
                    {
                        Directory.Delete(link.Source);
                        OutputHelper.AppendLine(String.Format("{0} has been deleted", link.Source));
                    }
                }

                if (File.Exists(link.Source))
                {
                    MkLinkMessageResult result = MessageBoxHelper.SourceFileExists(link);

                    if (result == MkLinkMessageResult.Cancel)
                    {
                        OutputHelper.AppendLine(String.Format("{0} links created before cancellation", createdCount));
                        return;
                    }
                    if (result == MkLinkMessageResult.Skip)
                    {
                        continue;
                    }
                    if (result == MkLinkMessageResult.Delete)
                    {
                        File.Delete(link.Source);
                        OutputHelper.AppendLine(String.Format("{0} has been deleted", link.Source));
                    }
                }

                bool success = MkLinkHelper.CreateLink(link);

                if (success)
                {
                    createdCount++;
                    OutputHelper.AppendLine("Link created successfully");
                }
                else
                {
                    OutputHelper.AppendLine("Link unsuccessful");
                }
            }

            OutputHelper.AppendLine(String.Format("All links completed, Created: {0}, Failed: {1}", createdCount, total - createdCount));
        }

        #endregion

        #endregion
    }

    
}