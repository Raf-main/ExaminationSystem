using ExaminationSystem.DAL.Entities;
using ExaminationSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.DAL.Repositories;

internal class MultipleQuestionRepository : GenericRepository<MultipleQuestionDbEntry>, IMultipleQuestionRepository
{
    public MultipleQuestionRepository(DbContext context) : base(context)
    {
    }
}