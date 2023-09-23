using Regna.Core.Context;
using Regna.Core.IServices;
using Regna.Core.Models;

namespace Regna.Core.Services
{
    public class TestService : ITestService
    {
        private readonly RegnaContext _dbContext;
        public TestService(RegnaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int GetRandomNumber()
        {
            return 342;
        }
        public void AddRandomUser()
        {
            _dbContext.Users.Add(new User { UserName = "a", TelegramId = 11, CreateDate = DateTime.Now });
            _dbContext.SaveChanges();
        }
    }
}
