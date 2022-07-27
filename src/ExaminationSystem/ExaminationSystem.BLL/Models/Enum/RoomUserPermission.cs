namespace ExaminationSystem.BLL.Models.Enum;

[Flags]
internal enum RoomUserPermission
{
    Chatting = 1,
    ExamParticipation = 2,
    DefaultUser = Chatting | ExamParticipation,
    ExamEditing = 4,
    RoleAssigning = 8,
    UserManaging = 16,
    Admin = ExamParticipation | ExamEditing | RoleAssigning | UserManaging
}