using ExaminationSystem.DAL.Entities;
using ExaminationSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.DAL.Repositories;

internal class ExamRepository : GenericRepository<ExamDbEntry>, IExamRepository
{
    public ExamRepository(DbContext context) : base(context)
    {
    }

    public new Task<ExamDbEntry> CreateAsync(ExamDbEntry entity)
    {
        return Task.FromResult(Table.Add(entity).Entity);
    }
}