using System.Linq;
using Sample.Common.Domain;
using Sample.Common.Extentions;
using Sample.Common.Heplers;
using Sample.Common.UserSessions;
using Sample.Domain.Geos.Cities;
using Sample.Domain.System.Users.Dtos;

namespace Sample.Domain.System.Users
{
    public partial class User
    {
        public static User Create(UserInput input, ISequenceProvider sequenceProvider, IUserSession userSession)
        {
            var entity = input.ToMapper<User>();
            entity.Id = sequenceProvider.GetGuidValue().Await().ToString();

            return entity;
        }

        public void Update(UserInput input)
        {
            var entity = this;
            MapHelper.Mapper(input, ref entity);
        }

        public void ChangePassword(string password, IUserSession _userSession)
        {
            var entity = this;
        }

        public UserInfo GetUserInfo()
        {
            var user = this;
            return new UserInfo()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive
            };
        }
    }
}