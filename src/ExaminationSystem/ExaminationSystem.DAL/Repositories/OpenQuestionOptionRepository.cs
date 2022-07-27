using ExaminationSystem.DAL.Entities;
using ExaminationSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.DAL.Repositories;

internal class OpenQuestionOptionRepository : GenericRepository<OpenQuestionOptionDbEntry>,
    IOpenQuestionOptionRepository
{
    public OpenQuestionOptionRepository(DbContext context) : base(context)
    {
    }
}