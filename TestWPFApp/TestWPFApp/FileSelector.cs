using Microsoft.Win32;

namespace TestWPFApp;

public class FileSelector
{
    public string SelectFile()
    {
        // Create an instance of the OpenFileDialog
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            // Set filter for file extension and default file extension
            DefaultExt = ".txt",
            Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
        };

        // Display OpenFileDialog by calling ShowDialog method
        bool? result = openFileDialog.ShowDialog();

        // Get the selected file name and display in a TextBox
        if (result == true)
            return openFileDialog.FileName;

        return string.Empty;
    }
}
