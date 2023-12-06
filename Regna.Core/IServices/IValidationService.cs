namespace Regna.Core.IServices
{
    public interface IValidationService
    {
        public bool ValidateAction(long userId, long matchId, long cardId, long targetId);
        public bool ValidatePlay(long userId, long matchId, long cardId);
        public bool ValidatePass(long userId, long matchId);
    }
}
