using System.IO;
using System.Windows;

namespace TestWPFApp
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

        public void SelectFileBtn_Click(object sender, RoutedEventArgs e)
        {
            string filePath = new FileSelector().SelectFile();

            if(string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                MessageBox.Show("No valid file selected!");
                return;
            }

            Inputs inputs = new FileConverter().GetInputsFromFile(filePath);

            string solution = new PuzzleSolver().SolvePuzzle(inputs);

            MessageBox.Show(solution);
        }
    }
}