using ExaminationSystem.DAL.Entities;
using ExaminationSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.DAL.Repositories;

internal class MultipleQuestionAnswerRepository : GenericRepository<MultipleQuestionAnswerDbEntry>,
    IMultipleQuestionAnswerRepository
{
    public MultipleQuestionAnswerRepository(DbContext context) : base(context)
    {
    }
}