using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AnkiTools.ExerciseImporter.Model;

namespace AnkiTools.ExerciseImporter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnChangeExerciseFilesDirectory_Click_1(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog browser = new FolderBrowserDialog())
            {
                var response = browser.ShowDialog();
                if (response == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(browser.SelectedPath))
                {
                    tbxExerciseFileDirectory.Text = browser.SelectedPath;
                }
            }
        }

        private void btnChangeAnkiDirectory_Click_1(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog browser = new FolderBrowserDialog())
            {
                var response = browser.ShowDialog();
                if (response == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(browser.SelectedPath))
                {
                    tbxAnkiDirectory.Text = browser.SelectedPath;
                }
            }
        }

        private void btnChangeAnkiImportFile_Click_1(object sender, RoutedEventArgs e)
        {
            using (SaveFileDialog saveFile = new SaveFileDialog())
            {
                var response = saveFile.ShowDialog();
                if (response == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(saveFile.FileName))
                {
                    tbxAnkiImportFile.Text = saveFile.FileName;
                }
            }
        }

        private void btnExport_Click_1(object sender, RoutedEventArgs e)
        {
            string topicDirectory = tbxExerciseFileDirectory.Text;
            string ankiDirectory = tbxAnkiDirectory.Text;
            string exportFile = tbxAnkiImportFile.Text;
            string title = tbxTopic.Text;

            Fachgebiet mathe = new Fachgebiet(title, topicDirectory, ankiDirectory);
            mathe.Export(exportFile);
        }
    }
}
