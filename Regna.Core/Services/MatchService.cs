using AutoMapper;
using Regna.Core.Context;
using Regna.Core.IServices;
using Regna.Core.Models;
using Regna.VM;
using Regna.VM.Enums;

namespace Regna.Core.Services
{
    public class MatchService : IMatchService
    {
        private readonly RegnaContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IValidationService _validator;
        private static Random rng = new Random();

        public MatchService(RegnaContext dbContext, IMapper mapper, IValidationService validator)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _validator = validator;
        }
        public bool StartMatch(long FirstPlayerId, long SecondPlayerId)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var p1 = _dbContext.Users.Where(a => a.UserId == FirstPlayerId).First();
                    var p2 = _dbContext.Users.Where(a => a.UserId == SecondPlayerId).First();
                    if (p1 == null || p2 == null)
                        throw new Exception();
                    var match = new Match
                    {
                        FirstPlayerId = FirstPlayerId,
                        SecondPlayerId = SecondPlayerId,
                        Phase = GamePhase.PlayPhase,
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
                    var cardinDecks = _dbContext.Deck.Where(a => a.UserId == FirstPlayerId || a.UserId == SecondPlayerId).ToList();
                    var OCardIds = cardinDecks.Select(a => a.OCardId);
                    var OCards = _dbContext.OCards.Where(a => OCardIds.Contains(a.OCardId)).ToList();
                    var cards = new List<Card>();
                    foreach (var cardInDeck in cardinDecks)
                    {
                        var ocard = OCards.Where(a => a.OCardId == cardInDeck.OCardId).First();
                        cards.Add(new Card
                        {
                            CardName = ocard.OCardName,
                            Location = CardLocation.Deck,
                            MatchId = match.MatchId,
                            OCardId = ocard.OCardId,
                            UserId = cardInDeck.UserId
                        });
                    }
                    cards.Shuffle();
                    _dbContext.Cards.AddRange(cards);
                    _dbContext.SaveChanges();
                    transaction.Commit();
                    Draw(5, FirstPlayerId, match.MatchId);
                    Draw(5, SecondPlayerId, match.MatchId);
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            return false;
        }
        public ResponseVM Play(long userId, long matchId, long cardId)
        {
            if (!_validator.ValidatePlay(userId, matchId, cardId))
            {
                return null;
            }
            var matchVM = GetMatch(matchId);
            //using (var transaction = _dbContext.Database.BeginTransaction())
            //{
            try
            {
                var cardVM = matchVM.Cards.Where(c => c.CardId == cardId).First();
                cardVM.Location = CardLocation.Ground;
                var res = new ResponseVM
                {
                    ChildResponses = new List<ResponseVM>(),
                    Actor = cardVM,
                    ResponseResultType = ResponseResultType.Played,
                };
                MechService.RunMech(res, cardVM, Listener.OnPlay, matchVM);
                //_dbContext.SaveChanges();
                //transaction.Commit();
                SaveMatch(matchVM, res);
                return res;
            }
            catch (Exception)
            {
                //transaction.Rollback();
            }
            //}
            return null;
        }
        public ResponseVM Action(long userId, long matchId, long cardId, long targetId)
        {
            if (!_validator.ValidateAction(userId, matchId, cardId, targetId))
            {
                return null;
            }
            var matchVM = GetMatch(matchId);
            var cardVM = matchVM.Cards.Where(c => c.CardId == cardId).First();
            var res = new ResponseVM
            {
                Actor = cardVM,
                ChildResponses = new List<ResponseVM>(),
                ResponseResultType = ResponseResultType.Actioned,

            };
            MechService.RunMech(res, cardVM, Listener.OnAction, matchVM, targetId: targetId);
            SaveMatch(matchVM, res);
            return res;
        }
        public ResponseVM Pass(long playerId, long matchId)
        {
            _validator.ValidatePass(playerId, matchId);
            var res = new ResponseVM();
            try
            {
                var match = _dbContext.Matches.Where(a => a.MatchId == matchId).First();
                if (match.FirstPlayerId == playerId)
                {
                    if (match.SecondPlayerPassed)
                    {
                        if (match.Phase == GamePhase.ActionPhase)
                        {
                            match.Phase = GamePhase.PlayPhase;
                            match.FirstPlayerRS += 10;
                            match.SecondPlayerRS += 10;
                        }
                        if (match.Phase == GamePhase.ActionPhase)
                        {
                            match.Phase = GamePhase.PlayPhase;

                        }
                        match.FirstPlayerPassed = false;
                        match.SecondPlayerPassed = false;
                    }
                    match.FirstPlayerPassed = true;
                }
                if (match.SecondPlayerId== playerId)
                {
                    if (match.FirstPlayerPassed)
                    {
                        if (match.Phase == GamePhase.ActionPhase)
                        {
                            match.Phase = GamePhase.PlayPhase;
                            match.FirstPlayerRS += 10;
                            match.SecondPlayerRS += 10;
                        }
                        if (match.Phase == GamePhase.ActionPhase)
                        {
                            match.Phase = GamePhase.PlayPhase;

                        }
                        match.FirstPlayerPassed = false;
                        match.SecondPlayerPassed = false;
                    }
                    match.SecondPlayerPassed = true;
                }
                res.ResponseResultType = ResponseResultType.Passed;
            }
            catch (Exception)
            {

            }
            return null;
        }

        bool Draw(int number, long userId, long matchId)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var cards = _dbContext.Cards.Where(a => a.MatchId == matchId && a.UserId == userId).ToList();
                    cards.Shuffle();
                    cards.Take(number);
                    foreach (var card in cards)
                    {
                        card.Location = CardLocation.Hand;
                    }
                    _dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            return false;
        }
        MatchVM GetMatch(long matchId)
        {
            var matchVM = new MatchVM();
            try
            {
                var cards = _mapper.Map<List<CardVM>>(_dbContext.Cards.Where(c => c.MatchId == matchId).ToList());
                var cardIds = cards.Select(a => a.CardId).ToList();
                var OcardIds = cards.Select(a => a.OCardId).ToList();
                var variables = _mapper.Map<List<VariableVM>>(_dbContext.Variables.Where(a => cardIds.Contains(a.CardId)).ToList());
                var mechOcards = _mapper.Map<List<MechOCardVM>>(_dbContext.MechOCards.Where(a => OcardIds.Contains(a.OCardId)));
                var mechIds = mechOcards.Select(a => a.MechanicId).ToList();
                var mechs = _mapper.Map<List<MechanicVM>>(_dbContext.Mechanics.Where(a => mechIds.Contains(a.MechanicId)));
                var events = _mapper.Map<List<EventVM>>(_dbContext.Events.Where(a => mechIds.Contains(a.MechanicId)));
                var conditions = _mapper.Map<List<ConditionVM>>(_dbContext.Conditions.Where(a => mechIds.Contains(a.MechanicId)));
                var listeners = _dbContext.ListenerMechs.Where(a => mechIds.Contains(a.MechanicId)).ToList();
                foreach (var mech in mechs)
                {
                    mech.Conditions = conditions.Where(a => a.MechanicId == mech.MechanicId).ToList();
                    mech.Events = events.Where(a => a.MechanicId == mech.MechanicId).ToList();
                    mech.Listeners = listeners.Where(a => a.MechanicId == mech.MechanicId).Select(a => a.Listener).ToList();
                }
                foreach (var card in cards)
                {
                    var mechsForThisCardIds = mechOcards.Where(a => a.OCardId == card.OCardId).Select(a => a.MechanicId).ToList();
                    card.Mechanics = mechs.Where(a => mechsForThisCardIds.Contains(a.MechanicId)).ToList();
                    card.Variables = variables.Where(a => a.CardId == card.CardId).ToList();
                }
                matchVM = _mapper.Map<MatchVM>(_dbContext.Matches.Where(a => a.MatchId == matchId && !a.IsDeleted).First());
                matchVM.Cards = cards;
                matchVM.FirstPlayer = _mapper.Map<UserVM>(_dbContext.Users.Where
                    (a => a.UserId == matchVM.FirstPlayerId && !a.IsDeleted).First());
                matchVM.SecondPlayer = _mapper.Map<UserVM>(_dbContext.Users.Where
                    (a => a.UserId == matchVM.SecondPlayerId && !a.IsDeleted).First());
                return matchVM;
            }
            catch (Exception)
            {

            }
            return null;
        }
        bool SaveMatch(MatchVM matchVM, ResponseVM rootResponse)
        {
            var responseList = ResponseTreeToList(rootResponse);
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var match = _dbContext.Matches.Where(a => a.MatchId == matchVM.MatchId && !a.IsDeleted).First();
                    foreach (var response in responseList)
                    {
                        switch (response.ResponseResultType)
                        {
                            case ResponseResultType.Played:
                                {
                                    if (response.Actor.UserId == match.FirstPlayerId)
                                    {
                                        match.FirstPlayerRS -= Convert.ToDouble(response.Actor.Variables.Where(a => a.VariableName == "RC").First().VariableValue);
                                    }
                                    else
                                    {
                                        match.SecondPlayerRS -= Convert.ToDouble(response.Actor.Variables.Where(a => a.VariableName == "RC").First().VariableValue);
                                    }
                                    var card = _dbContext.Cards.Where(a => a.CardId == response.Actor.CardId).First();
                                    card.Location = CardLocation.Ground;
                                    _dbContext.SaveChanges();
                                }
                                break;
                            case ResponseResultType.Actioned:
                                {
                                    if (response.Actor.UserId == match.FirstPlayerId)
                                    {
                                        match.FirstPlayerRS -= Convert.ToDouble(response.Actor.Variables.Where(a => a.VariableName == "RC").First().VariableValue);
                                    }
                                    else
                                    {
                                        match.SecondPlayerRS -= Convert.ToDouble(response.Actor.Variables.Where(a => a.VariableName == "RC").First().VariableValue);
                                    }
                                    _dbContext.SaveChanges();
                                }
                                break;
                            case ResponseResultType.Damaged:
                            case ResponseResultType.Healed:
                            case ResponseResultType.VariableChanged:
                                {
                                    var variable = _dbContext.Variables.Where(a => a.VariableId == response.Target_Variable.VariableId).First();
                                    variable.VariableValue = response.Target_Variable.VariableValue;
                                    _dbContext.SaveChanges();
                                }
                                break;
                            case ResponseResultType.Killed:
                                {
                                    var variable = _dbContext.Variables.Where(a => a.VariableId == response.Target_Variable.VariableId).First();
                                    variable.VariableValue = response.Target_Variable.VariableValue;
                                    if (response.Target.UserId == match.FirstPlayerId)
                                    {
                                        match.FirstPlayerMorale -= Convert.ToDouble(response.Target.Variables.Where(a => a.VariableName == "MP").First().VariableValue);
                                    }
                                    else
                                    {
                                        match.FirstPlayerMorale -= Convert.ToDouble(response.Target.Variables.Where(a => a.VariableName == "MP").First().VariableValue);
                                    }
                                    var card = _dbContext.Cards.Where(a => a.CardId == response.Target.CardId).First();
                                    card.Location = CardLocation.Graveyard;
                                    _dbContext.SaveChanges();
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    //_dbContext.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
            //return true;
        }
        public List<ResponseVM> ResponseTreeToList(ResponseVM node)
        {

            var allResponses = new List<ResponseVM>();

            // Add current node
            allResponses.Add(node);

            // Recursively get child responses
            foreach (var child in node.ChildResponses)
            {
                allResponses.AddRange(ResponseTreeToList(child));
            }

            return allResponses;

        }
    }
}
