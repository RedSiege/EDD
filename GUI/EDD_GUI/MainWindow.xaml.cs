using EDDLib;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EDD_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FunctionList = funcLists.CreateFunctionList();
            ForestList = funcLists.CreateForestList();
            UserInfoList = funcLists.CreateUserInfoList();
            CompInfoList = funcLists.CreateCompInfoList();
            ChainedInfoList = funcLists.CreateChainedList();
            FunctionCombo.ItemsSource = FunctionList;
        }

        private void FunctionCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FunctionCombo.SelectedIndex == 0)
                FunctionOptionListBox.ItemsSource = ForestList;
            else if (FunctionCombo.SelectedIndex == 1)
                FunctionOptionListBox.ItemsSource = CompInfoList;
            else if (FunctionCombo.SelectedIndex == 2)
                FunctionOptionListBox.ItemsSource = UserInfoList;
            else if (FunctionCombo.SelectedIndex == 3)
                FunctionOptionListBox.ItemsSource = ChainedInfoList;
        }

        #region Misc
        internal static string SaveFilePath;
        internal static string SaveFileName;
        List<string> ForestList = new List<string>();
        List<string> CompInfoList = new List<string>();
        List<string> UserInfoList = new List<string>();
        List<string> ChainedInfoList = new List<string>();
        List<string> FunctionList = new List<string>();
        FunctionOptionLists funcLists = new FunctionOptionLists();
        #endregion

        #region Arguments
        internal static string targetDomain = "--domainame=";
        internal static string targetUser = "--username=";
        internal static string taregtPass = "--password=";
        internal static string targetConical = "--canonicalname=";
        internal static string targetShare = "--sharepath=";
        internal static string targetGroup = "--groupname=";
        internal static string targetProcess = "--processname=";
        internal static string targetComputer = "--computername=";
        internal static string targetFunction = "--function=";
        #endregion

        private void OutputButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = ".txt (.txt) | *.txt";
            saveFile.OverwritePrompt = true;
            saveFile.ShowDialog();
            SaveFileName = saveFile.FileName;
            SaveFilePath = System.IO.Path.GetFullPath(saveFile.FileName);
            OutputTextBox.Text = SaveFilePath;
        }

        internal async void RunEDD()
        {
            List<string> argList = new List<string>();
            if (TargetDomainName.Text != "")
                argList.Add($"{targetDomain}{TargetDomainName.Text}");
            if (TargetCompName.Text != "")
                argList.Add($"{targetComputer}{TargetCompName.Text}");
            if (TargetConicalName.Text != "")
                argList.Add($"{targetConical}{TargetConicalName.Text}");
            if (TargetGroupName.Text != "")
                argList.Add($"{targetGroup}{TargetGroupName.Text}");
            if (TargetPassword.Text != "")
                argList.Add($"{TargetPassword}{TargetPassword.Text}");
            if (TargetSharePath.Text != "")
                argList.Add($"{targetShare}{TargetSharePath.Text}");
            if (TargetUsername.Text != "")
                argList.Add($"{targetUser}{TargetUsername.Text}");
            if (TargetProcessName.Text != "")
                argList.Add($"{targetProcess}{TargetProcessName.Text}");
            argList.Add($"{targetFunction}{FunctionOptionListBox.SelectedItem.ToString()}");
            string[] arguments = argList.ToArray();
            EDDRuntime.Main(arguments);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Thread runThread = new Thread(RunEDD);
            runThread.Start();
        }
    }
}
