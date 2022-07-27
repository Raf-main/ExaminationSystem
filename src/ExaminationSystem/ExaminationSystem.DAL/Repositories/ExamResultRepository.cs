using ExaminationSystem.DAL.Entities;
using ExaminationSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.DAL.Repositories;

internal class ExamResultRepository : GenericRepository<ExamResultDbEntry>, IExamResultRepository
{
    public ExamResultRepository(DbContext context) : base(context)
    {
    }
}