using ExaminationSystem.BLL.Models;

namespace ExaminationSystem.BLL.Managers.RoomManagers;

public interface IRoomInviteManager
{
    public Task AcceptInviteAsync(int inviteId, string userId, int roomId);
    public Task<int> CreateInviteAsync(string fromUserId, string toUserId, int roomId);
    public Task<IEnumerable<Invite>> GetReceivedInvitesAsync(string userId);
    public Task<Invite?> GetInviteAsync(int inviteId);
    public Task<bool> HasUserManagingPermissionAsync(string userId, int roomId);
}