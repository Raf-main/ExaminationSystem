namespace ExaminationSystem.DAL.Repositories.Interfaces;

public interface IUnitOfWork
{
    IRoomRepository Rooms { get; }
    IRoomUserRepository RoomUsers { get; }
    IRoomMessageRepository RoomMessages { get; }
    IRoomInviteRepository Invites { get; }
    IExamRepository Exams { get; }
    IExamResultRepository ExamResults { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    IMultipleQuestionRepository MultipleQuestions { get; }
    IMultipleQuestionOptionRepository MultipleQuestionOptions { get; }
    IMultipleQuestionAnswerRepository MultipleQuestionAnswers { get; }
    IMultipleQuestionOptionAnswerRepository MultipleQuestionOptionAnswers { get; }
    IOpenQuestionRepository OpenQuestions { get; }
    IOpenQuestionOptionRepository OpenQuestionOptions { get; }
    Task SaveChangesAsync();
}