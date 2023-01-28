namespace ChatApplication.FileIO;

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;

class FileHandler
{
    public async Task WriteToFileAsync(ObservableCollection<string> content)
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        if (saveFileDialog.ShowDialog() == true)
        {
            using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName, false))
            {
                foreach (string line in content)
                {
                    await writer.WriteLineAsync(line);
                }
            }
        }
    }

    public async Task<ObservableCollection<string>> ReadFromFileAsync()
    {
        var lines = new ObservableCollection<string>();
        OpenFileDialog openFileDialog = new OpenFileDialog();
        if (openFileDialog.ShowDialog() == true)
        {
            using (StreamReader reader = new StreamReader(openFileDialog.FileName))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    lines.Add(line);
                }
            }
        }

        return lines;
    }
}