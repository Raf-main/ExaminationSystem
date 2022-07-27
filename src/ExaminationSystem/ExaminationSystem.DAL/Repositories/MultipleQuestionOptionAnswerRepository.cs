using ExaminationSystem.DAL.Entities;
using ExaminationSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.DAL.Repositories;

internal class MultipleQuestionOptionAnswerRepository : GenericRepository<MultipleQuestionOptionAnswerDbEntry>,
    IMultipleQuestionOptionAnswerRepository
{
    public MultipleQuestionOptionAnswerRepository(DbContext context) : base(context)
    {
    }
}