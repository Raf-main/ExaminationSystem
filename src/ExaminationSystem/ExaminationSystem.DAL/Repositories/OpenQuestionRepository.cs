using ExaminationSystem.DAL.Entities;
using ExaminationSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.DAL.Repositories;

internal class OpenQuestionRepository : GenericRepository<OpenQuestionDbEntry>, IOpenQuestionRepository
{
    public OpenQuestionRepository(DbContext context) : base(context)
    {
    }
}