using ExaminationSystem.DAL.Entities;
using ExaminationSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.DAL.Repositories;

internal class MultipleQuestionOptionRepository : GenericRepository<MultipleQuestionOptionDbEntry>,
    IMultipleQuestionOptionRepository
{
    public MultipleQuestionOptionRepository(DbContext context) : base(context)
    {
    }
}