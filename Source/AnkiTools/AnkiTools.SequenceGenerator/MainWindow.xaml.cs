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
using AnkiTools.SequenceGenerator.Model;

namespace AnkiTools.SequenceGenerator
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

        private void btnChange_Click_1(object sender, RoutedEventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                var response = dialog.ShowDialog();
                if (response == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.FileName))
                {
                    tbxFilename.Text = dialog.FileName;
                }
            }
        }

        private void btnExport_Click_1(object sender, RoutedEventArgs e)
        {
            string title = tbxTitle.Text ?? string.Empty;
            string category = tbxCategory.Text ?? string.Empty;
            string subcategory = tbxSubcategory.Text ?? string.Empty;
            string source = tbxSource.Text ?? string.Empty;
            string steps = tbxSteps.Text ?? string.Empty;
            string filename = tbxFilename.Text;
            Derivation derivation = new Derivation(title, category, subcategory, source);
            string[] splitSteps = steps.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in splitSteps)
            {
                derivation.AddStep(item);
            }
            derivation.CreateAllCards(filename);
        }
    }
}
