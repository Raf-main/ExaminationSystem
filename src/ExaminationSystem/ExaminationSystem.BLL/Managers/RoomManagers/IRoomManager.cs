using ExaminationSystem.BLL.Models;
using ExaminationSystem.BLL.Models.Create;
using ExaminationSystem.DAL.Helpers;

namespace ExaminationSystem.BLL.Managers.RoomManagers;

public interface IRoomManager
{
    Task<Room?> GetRoomByIdAsync(int id);
    IEnumerable<Room> GetRoomsByUserEmail(string email);
    IEnumerable<Room> GetOwnedRooms(string email);
    Task<int> CreateRoomAsync(RoomCreateData roomData);
    bool IsRoomOwner(string userId, int roomId);
    bool IsRoomMember(string userId, int roomId);

    Task AddUserToRoomAsync(string userIdToBeAdded, int roomId,
        RoomPermission permissions = RoomPermission.DefaultUser);

    Task DeleteUserFromRoomAsync(string userIdToBeDeleted, int roomId);
    Task<RoomPermission> GetRoomUserPermissionsAsync(string userId, int roomId);
}