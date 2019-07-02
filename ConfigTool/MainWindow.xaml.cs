//using System.Windows.Forms;
using System.Windows;
using System.IO;
using System;
using System.Drawing;
namespace ConfigTool
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() {
            InitializeComponent();
        }
        
        private void InputButton_Click(object sender, RoutedEventArgs e) {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                InputBox.Text = openFileDialog.FileName;
                OutputBox.Text = openFileDialog.FileName.Substring(0, openFileDialog.FileName.LastIndexOf('\\'));
            }
        }

        private void OutputButton_Click(object sender, RoutedEventArgs e) {
            System.Windows.Forms.FolderBrowserDialog openFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (openFolderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                OutputBox.Text = openFolderDialog.SelectedPath;
            }
        }

        private void LevelsButton_Click(object sender, RoutedEventArgs e) {
            string input = InputBox.Text;
            string output = OutputBox.Text;
            var cp = new ConfigParser(input, output);
        }
    }
}
