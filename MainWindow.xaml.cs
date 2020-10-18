using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

namespace COD4SaveTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SaveHeader saveObj;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOpenSaveFile_Click(object sender, RoutedEventArgs e)
        {
            txtGameScript.Document.Blocks.Clear();

            OpenFileDialog dialog = new OpenFileDialog();

            if ((bool)dialog.ShowDialog())
            {
                //saveObj.LittleEndian = false;

                try
                {
                    saveObj = new SaveHeader(dialog.FileName);
                    saveObj.LittleEndian = radioPC.IsChecked.Value;

                    saveObj.ParseGameScripts();
                    txtGameScript.Document.Blocks.Add(new Paragraph(new Run(saveObj.gamescripts)));

                    labelFileName.Content = dialog.SafeFileName;

                    MessageBox.Show("Save game loaded");
                }
                catch
                {
                    MessageBox.Show("An exception was thrown when trying to parse this savegame. Please make sure that you are using a valid debug save game file for with the correct platform selected in the tool.");
                }


                
            }

            
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(saveObj != null)
            {
                saveObj.SaveGameScripts();
            }
        }

        private void radioPC_Checked(object sender, RoutedEventArgs e)
        {
            if (saveObj != null)
            {
                saveObj.LittleEndian = radioPC.IsChecked.Value;
            }
        }

        private void radioXbox_Checked(object sender, RoutedEventArgs e)
        {
            if (saveObj != null)
            {
                saveObj.LittleEndian = radioPC.IsChecked.Value;
            }
        }
    }
}
