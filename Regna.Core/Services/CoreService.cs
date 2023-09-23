using Regna.Core.IServices;
using Regna.VM;

namespace Regna.Core.Services
{
    public class CoreService
    {
        public  ITestService _testService { get; }
        public CoreService(ITestService testService)
        {
            _testService = testService;
        }
        //public UserVM AddUser(UserVM userVM)
        //{
        //    return null;
        //}
    }
}
