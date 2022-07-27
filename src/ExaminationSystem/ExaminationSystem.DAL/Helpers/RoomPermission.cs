namespace ExaminationSystem.DAL.Helpers;

[Flags]
public enum RoomPermission
{
    Chatting = 1,
    ExamParticipation = 2,
    ExamEditing = 4,
    RoleAssigning = 8,
    UserManaging = 16,
    DefaultUser = Chatting | ExamParticipation,
    Admin = DefaultUser | ExamEditing | RoleAssigning | UserManaging
}