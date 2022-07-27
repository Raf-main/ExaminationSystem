using System.Text;

namespace ExaminationSystem.BLL.Managers.FileManagers;

internal interface IFileManager
{
    public TextReader CreateTextReader(string filePath, Encoding? encoding = null);
    public TextWriter CreateTextWriter(string filePath);
}