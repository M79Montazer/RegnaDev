using Regna.VM;

namespace Regna.Core.IServices
{
    public interface IMatchService
    {
        bool StartMatch(long FirstPlayerId, long SecondPlayerId);

        ResponseVM Play(long userId, long matchId, long cardId);
        ResponseVM Action(long userId, long matchId, long cardId, long targetId);
        ResponseVM Pass(long playerId, long matchId);
        MatchVM GetMatchVM(long playerId, long matchId);
        bool ClearBoard(long matchId);
    }
}
