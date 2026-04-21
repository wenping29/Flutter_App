using ChatServer.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatServer.DAL
{
    /// <summary>
    /// 好友数据访问层
    /// </summary>
    public class FriendDAL
    {
        private readonly AppDbContext _dbContext;

        public FriendDAL(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 查询当前用户的好友列表（已通过）
        /// </summary>
        /// <param name="userId">当前用户ID</param>
        /// <returns>好友列表（包含用户基础信息）</returns>
        public async Task<List<dynamic>> GetFriendListAsync(int userId)
        {
            // 联表查询：好友关系表 + 用户表
            var friendList = await _dbContext.Friends
                .Where(f => f.FriendUserId == userId && f.Status == 1)
                // .Join(
                //     _dbContext.Users, // 关联用户表
                //     f => f.UserId, // 好友关系表的好友ID
                //     u => u.Id, // 用户表的用户ID
                //     (f, u) => new // 拼接返回结果
                //     {
                //         FriendId = f.UserId,
                //         Username = u.Username,
                //         Avatar = u.Avatar ?? "/assets/avatar_default.png",
                //         RemarkName = f.RemarkName,
                //         AddTime = f.CreateTime.ToString("yyyy-MM-dd"),
                //         Phone = u.Phone
                //     }
                // ).OrderByDescending(f => f.AddTime)
                .ToListAsync();
            

            // 转换为dynamic便于JSON序列化
            return friendList.Select(f => new
            {
                // f.FriendId,
                // f.Username,
                // f.Avatar,
                // f.RemarkName,
                // f.AddTime,
                // f.Phone
                    FriendId = f.UserId,
                    Username = f.UserId,
                    Avatar =   "/avatar_default.png",
                    RemarkName = f.RemarkName,
                    AddTime = f.CreateTime.ToString("yyyy-MM-dd"),
                    Phone = "12345678900",
            }).Cast<dynamic>().ToList();
        }
    }
}