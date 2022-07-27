using ExaminationSystem.DAL.Entities;

namespace ExaminationSystem.DAL.Repositories.Interfaces;

public interface IExamRepository : IGenericRepository<ExamDbEntry>
{
    new Task<ExamDbEntry> CreateAsync(ExamDbEntry entity);
}