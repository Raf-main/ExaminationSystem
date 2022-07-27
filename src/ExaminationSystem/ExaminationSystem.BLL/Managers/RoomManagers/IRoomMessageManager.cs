using ExaminationSystem.BLL.Helpers;
using ExaminationSystem.BLL.Models;
using ExaminationSystem.BLL.Models.Create;

namespace ExaminationSystem.BLL.Managers.RoomManagers;

public interface IRoomMessageManager
{
    IEnumerable<RoomMessage> GetMessages(int roomId, GenericRange<int> range);
    Task<RoomMessage> AddMessageAsync(RoomMessageCreateData createData);
    Task<bool> HasChattingPermissionAsync(string userId, int roomId);
}