namespace ExaminationSystem.BLL.Helpers;

public interface IDateTimeProvider
{
    public DateTime GetUtcNow();
}