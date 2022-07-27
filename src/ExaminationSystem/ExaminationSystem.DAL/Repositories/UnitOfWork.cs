using ExaminationSystem.DAL.Contexts;
using ExaminationSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExaminationSystem.DAL.Repositories;

public class UnitOfWork : DbContext, IUnitOfWork
{
    private readonly ExaminationDbContext _dbContext;
    private readonly ILogger<UnitOfWork> _logger;

    public UnitOfWork(ILogger<UnitOfWork> logger, ExaminationDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }

    public IExamRepository Exams => _examRepository ?? new ExamRepository(_dbContext);
    public IRoomRepository Rooms => _roomRepository ?? new RoomRepository(_dbContext);
    public IRoomUserRepository RoomUsers => _roomUserRepository ?? new RoomUserRepository(_dbContext);
    public IRoomMessageRepository RoomMessages => _roomMessageRepository ?? new RoomMessageRepository(_dbContext);
    public IRoomInviteRepository Invites => _roomInviteRepository ?? new RoomInviteRepository(_dbContext);
    public IExamResultRepository ExamResults => _examResultRepository ?? new ExamResultRepository(_dbContext);
    public IRefreshTokenRepository RefreshTokens => _refreshTokenRepository ?? new RefreshTokenRepository(_dbContext);

    public IMultipleQuestionRepository MultipleQuestions =>
        _multipleQuestionRepository ?? new MultipleQuestionRepository(_dbContext);

    public IMultipleQuestionOptionRepository MultipleQuestionOptions =>
        _multipleQuestionOptionRepository ?? new MultipleQuestionOptionRepository(_dbContext);

    public IMultipleQuestionAnswerRepository MultipleQuestionAnswers =>
        _multipleQuestionAnswerRepository ?? new MultipleQuestionAnswerRepository(_dbContext);

    public IMultipleQuestionOptionAnswerRepository MultipleQuestionOptionAnswers =>
        _multipleQuestionOptionAnswerRepository ?? new MultipleQuestionOptionAnswerRepository(_dbContext);

    public IOpenQuestionRepository OpenQuestions => _openQuestionRepository ?? new OpenQuestionRepository(_dbContext);

    public IOpenQuestionOptionRepository OpenQuestionOptions =>
        _openQuestionOptionRepository ?? new OpenQuestionOptionRepository(_dbContext);

    public async Task SaveChangesAsync()
    {
        try
        {
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Changes where successfully saved to the database");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Saving changes to the database passed with error");

            throw;
        }
    }

#pragma warning disable CS0649
    private readonly IRoomRepository? _roomRepository;
    private readonly IRoomUserRepository? _roomUserRepository;
    private readonly IRoomInviteRepository? _roomInviteRepository;
    private readonly IRoomMessageRepository? _roomMessageRepository;
    private readonly IExamRepository? _examRepository;
    private readonly IExamResultRepository? _examResultRepository;
    private readonly IRefreshTokenRepository? _refreshTokenRepository;
    private readonly IMultipleQuestionRepository? _multipleQuestionRepository;
    private readonly IMultipleQuestionOptionRepository? _multipleQuestionOptionRepository;
    private readonly IMultipleQuestionAnswerRepository? _multipleQuestionAnswerRepository;
    private readonly IMultipleQuestionOptionAnswerRepository? _multipleQuestionOptionAnswerRepository;
    private readonly IOpenQuestionRepository? _openQuestionRepository;
    private readonly IOpenQuestionOptionRepository? _openQuestionOptionRepository;
#pragma warning restore CS0649
}