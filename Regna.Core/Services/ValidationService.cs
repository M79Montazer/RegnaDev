using Regna.Core.IServices;

namespace Regna.Core.Services
{
    public class ValidationService : IValidationService
    {

        public bool ValidatePlay(long userId, long matchId, long cardId)
        {
            return true;
        }
        public bool ValidateAction(long userId, long matchId, long cardId, long targetId)
        {
            return true;
        }
        public bool ValidatePass(long userId, long matchId)
        {
            return true;
        }
    }
}
