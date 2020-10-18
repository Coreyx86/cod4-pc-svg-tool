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
        private string savefile = "killhouse.svg";
        private SaveHeader saveObj;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            saveObj = new SaveHeader(savefile);

            saveObj.ParseGameScripts();
        }

        private void btnOpenSaveFile_Click(object sender, RoutedEventArgs e)
        {
            saveObj = new SaveHeader(savefile);

            saveObj.ParseGameScripts();

            txtGameScript.Document.Blocks.Add(new Paragraph(new Run(saveObj.gamescripts)));

            labelFileName.Content = savefile;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
