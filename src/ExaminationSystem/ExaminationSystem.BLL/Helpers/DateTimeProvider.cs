namespace ExaminationSystem.BLL.Helpers;

internal class DateTimeProvider : IDateTimeProvider
{
    public DateTime GetUtcNow()
    {
        return DateTime.UtcNow;
    }
}