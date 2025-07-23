using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.ExternalServices.Hubs;

public class CommentHub : Hub
{
    private readonly ILogger<CommentHub> _logger;
    private readonly UserManager<User> _userManager;
    public CommentHub(ILogger<CommentHub> logger, UserManager<User> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public override Task OnConnectedAsync()
    {
        // Có thể xử lý logic nếu cần
        _logger.LogInformation($"User connected: {Context.ConnectionId}");
        return base.OnConnectedAsync();
    }
    // Tham gia nhóm comment theo loại (movie hoặc episode) và ID
    public async Task JoinGroup(string targetType, int targetId)
    {
        var groupName = GetGroupName(targetType, targetId);
        _logger.LogInformation($"User {Context.ConnectionId} joining group {groupName}");
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    // Rời khỏi nhóm khi không còn xem video đó nữa
    public async Task LeaveGroup(string targetType, int targetId)
    {
        var groupName = GetGroupName(targetType, targetId);
        _logger.LogInformation($"User {Context.ConnectionId} leaving group {groupName}");
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

    // Gửi comment mới đến các client trong cùng nhóm
    public async Task SendComment(string targetType, int targetId, string message)
    {
        var groupName = GetGroupName(targetType, targetId);
        var user = await _userManager.GetUserAsync(Context.User);
        if (user == null) return;

        var comment = new
        {
            User = user.FullName,
            Content = message,
            CreatedAt = DateTime.UtcNow,
            Avatar =  String.IsNullOrEmpty(user.AvatarUrl) ? "/img/anime/review-1.jpg" : user.AvatarUrl,
        };

        await Clients.Group(groupName).SendAsync("ReceiveComment", comment);
    }

    public async Task SendReplyComment(string targetType, int targetId, string message, int? parentId = null)
    {
        var groupName = GetGroupName(targetType, targetId);
        var user = await _userManager.GetUserAsync(Context.User);
        if (user == null) return;

        var comment = new
        {
            Id = Guid.NewGuid().ToString(), // hoặc dùng từ DB nếu có
            User = user.FullName,
            Content = message,
            CreatedAt = DateTime.UtcNow,
            Avatar = String.IsNullOrEmpty(user.AvatarUrl) ? "/img/anime/review-1.jpg" : user.AvatarUrl,
            ParentId = parentId
        };


        await Clients.Group(groupName).SendAsync("ReceiveComment", comment);
    }

    private string GetGroupName(string targetType, int targetId)
    {
        return $"{targetType.ToLower()}-{targetId}";
    }
}