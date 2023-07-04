using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleChatApi.Contexts;
using SimpleChatApi.Entities;
using SimpleChatApi.Models.Home;

namespace SimpleChatApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("GetAllMessages")]
        public IActionResult GetAllMessages()
        {
            if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value, out int userId))
            {
                return BadRequest("Invalid user ID");
            }

            var userMessages = _appDbContext.Chats.Where(c => c.SenderId == userId || c.RecipientId == userId)
                                                  .Select(c => new MessageModel
                                                  {
                                                      SenderId = c.SenderId,
                                                      SenderUsername = c.Sender.Username,
                                                      RecipientId = c.RecipientId,
                                                      RecipientUsername = c.Recipient.Username,
                                                      Content = c.Content,
                                                      CreatedAt = c.CreatedAt
                                                  })
                                                  .ToList();

            var groupMessages = _appDbContext.GroupChats.Where(gc => gc.SenderId == userId)
                                                        .Select(gc => new MessageModel
                                                        {
                                                            SenderId = gc.SenderId,
                                                            SenderUsername = gc.Sender.Username,
                                                            RecipientId = gc.GroupId,
                                                            RecipientUsername = gc.Group.Name,
                                                            Content = gc.Content,
                                                            CreatedAt = gc.CreatedAt
                                                        })
                                                        .ToList();

            var allMessages = userMessages.Concat(groupMessages)
                                          .OrderByDescending(m => m.CreatedAt)
                                          .ToList();

            return Ok(allMessages);
        }

        [HttpPost("SendMessage")]
        public IActionResult SendMessage(ChatModel chatDto)
        {
            if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value, out int userId))
            {
                return BadRequest("Invalid user ID");
            }

            var chat = new Chat
            {
                SenderId = userId,
                RecipientId = chatDto.RecipientId,
                Content = chatDto.Content,
                CreatedAt = DateTime.Now
            };

            _appDbContext.Chats.Add(chat);
            _appDbContext.SaveChanges();

            return Ok();
        }

        [HttpPost("SendGroupMessage")]
        public IActionResult SendGroupMessage(GroupChatModel groupChatDto)
        {
            if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value, out int userId))
            {
                return BadRequest("Invalid user ID");
            }

            var groupChat = new GroupChat
            {
                SenderId = userId,
                GroupId = groupChatDto.GroupId,
                Content = groupChatDto.Content,
                CreatedAt = DateTime.Now
            };

            _appDbContext.GroupChats.Add(groupChat);
            _appDbContext.SaveChanges();

            return Ok();
        }

        [HttpGet("GetUserGroups")]
        public IActionResult GetUserGroups()
        {
            if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value, out int userId))
            {
                return BadRequest("Invalid user ID");
            }

            var userGroups = _appDbContext.GroupUserss.Where(gu => gu.UserId == userId)
                                                      .Select(gu => gu.Group)
                                                      .ToList();

            return Ok(userGroups);
        }

        [HttpPost("CreateGroup")]
        public IActionResult CreateGroup(GroupModel groupDto)
        {
            if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value, out int userId))
            {
                return BadRequest("Invalid user ID");
            }

            var group = new Group
            {
                Name = groupDto.Name
            };

            _appDbContext.Groups.Add(group);
            _appDbContext.SaveChanges();

            var groupUser = new GroupUsers
            {
                GroupId = group.Id,
                UserId = userId
            };

            _appDbContext.GroupUserss.Add(groupUser);
            _appDbContext.SaveChanges();

            return Ok();
        }

        [HttpPost("AddUserToGroup")]
        public IActionResult AddUserToGroup(AddUserToGroupModel addUserToGroupDto)
        {
            var group = _appDbContext.Groups.FirstOrDefault(g => g.Id == addUserToGroupDto.GroupId);
            if (group == null)
            {
                return NotFound("Group not found");
            }

            var existingMembership = _appDbContext.GroupUserss.FirstOrDefault(gu => gu.GroupId == addUserToGroupDto.GroupId && gu.UserId == addUserToGroupDto.UserId);
            if (existingMembership != null)
            {
                return BadRequest("User is already a member of the group");
            }

            var membership = new GroupUsers
            {
                GroupId = addUserToGroupDto.GroupId,
                UserId = addUserToGroupDto.UserId
            };

            _appDbContext.GroupUserss.Add(membership);
            _appDbContext.SaveChanges();

            return Ok();
        }

    }
}
