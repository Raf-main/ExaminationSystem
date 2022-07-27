using System.Text;

namespace ExaminationSystem.BLL.Managers.FileManagers;

internal class FileManager : IFileManager
{
    public TextReader CreateTextReader(string filePath, Encoding? encoding = null)
    {
        return encoding != null ? new StreamReader(filePath, encoding) : new StreamReader(filePath);
    }

    public TextWriter CreateTextWriter(string filePath)
    {
        return new StreamWriter(filePath);
    }
}