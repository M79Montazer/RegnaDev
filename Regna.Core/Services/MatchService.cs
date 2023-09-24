using Regna.Core.Context;
using Regna.Core.IServices;
using Regna.Core.Models;
using Regna.VM.Enums;

namespace Regna.Core.Services
{
    public class MatchService : IMatchService
    {
        private readonly RegnaContext _dbContext;
        public MatchService(RegnaContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool StartMatch(long FirstPlayerId, long SecondPlayerId)
        {
            using (var transaction = _dbContext.Database.BeginTransaction()){
                try
                {
                    var p1 = _dbContext.Users.Where(a => a.UserId == FirstPlayerId).First();
                    var p2 = _dbContext.Users.Where(a => a.UserId == SecondPlayerId).First();
                    if (p1 != null || p2 != null)
                        throw new Exception();
                    var match = new Match
                    {
                        FirstPlayerId = FirstPlayerId,
                        SecondPlayerId = SecondPlayerId,
                        Phase = GamePhase.MatchStart,
                        PlayerTurn = 1,
                        FirstPlayerMorale = 50,
                        SecondPlayerMorale = 50,
                        FirstPlayerRS = 10,
                        SecondPlayerRS = 10,
                        CreateDate = DateTime.Now,
                        IsDeleted = false,
                    };
                    _dbContext.Matches.Add(match);


                    _dbContext.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            return false;
        }

    }
}
