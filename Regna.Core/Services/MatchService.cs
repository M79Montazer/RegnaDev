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
                        FirstPlayerMorale = 20,
                        SecondPlayerMorale = 20,
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
                            UserId = cardInDeck.UserId,
                            PositionNumber = 0
                        });
                    }
                    cards.Shuffle();
                    _dbContext.Cards.AddRange(cards);
                    _dbContext.SaveChanges();
                    var variables = new List<Variable>();
                    var oVariables = _dbContext.OVariables.Where(a => !a.IsDeleted && OCardIds.Contains(a.OCardID)).ToList();
                    foreach (var card in cards)
                    {
                        var ovs = oVariables.Where(a => a.OCardID == card.OCardId).ToList();
                        foreach (var ov in ovs)
                        {
                            variables.Add(new Variable
                            {
                                CardId = card.CardId,
                                VariableName = ov.OVariableName,
                                VariableType = ov.VariableType,
                                VariableValue = ov.InitialValue
                            });
                        }
                    }
                    _dbContext.Variables.AddRange(variables);
                    _dbContext.SaveChanges();
                    transaction.Commit();
                    var matchVM = GetMatch(match.MatchId);
                    Draw(5, FirstPlayerId, match.MatchId, matchVM);
                    Draw(5, SecondPlayerId, match.MatchId, matchVM);
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
                var firstAvailablePos = 0;
                var groundCardNumbers = matchVM.Cards.Where(a => a.UserId == userId && a.Location == CardLocation.Ground).Select(a => a.PositionNumber).OrderBy(a => a).ToList();
                if (groundCardNumbers is not null)
                    for (int i = 0; i < 5; i++)
                    {
                        if (groundCardNumbers.Contains(firstAvailablePos))
                        {
                            firstAvailablePos++;
                        }
                    }
                cardVM.PositionNumber = firstAvailablePos;
                cardVM.Location = CardLocation.Ground;
                var res = new ResponseVM
                {
                    ChildResponses = new List<ResponseVM>(),
                    Target = cardVM,
                    ResponseResultType = ResponseResultType.Played,
                };
                MechService.RunMech(res, cardVM, Listener.OnPlay, matchVM);
                //_dbContext.SaveChanges();
                //transaction.Commit();
                SaveMatch(matchVM, res);
                var drawedList = Draw(1, userId, matchId, matchVM);
                if (drawedList is not null)
                {

                    res.ChildResponses.Add(new ResponseVM
                    {
                        ResponseResultType = ResponseResultType.Drawed,
                        Target = drawedList.First()
                    });
                }
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
                        else if (match.Phase == GamePhase.PlayPhase)
                        {
                            match.Phase = GamePhase.ActionPhase;

                        }
                        match.FirstPlayerPassed = false;
                        match.SecondPlayerPassed = false;
                    }
                    else
                    {
                        match.FirstPlayerPassed = true;
                    }
                }
                if (match.SecondPlayerId == playerId)
                {
                    if (match.FirstPlayerPassed)
                    {
                        if (match.Phase == GamePhase.ActionPhase)
                        {
                            match.Phase = GamePhase.PlayPhase;
                            match.FirstPlayerRS += 10;
                            match.SecondPlayerRS += 10;
                        }
                        else if (match.Phase == GamePhase.PlayPhase)
                        {
                            match.Phase = GamePhase.ActionPhase;

                        }
                        match.FirstPlayerPassed = false;
                        match.SecondPlayerPassed = false;
                    }
                    else
                    {
                        match.SecondPlayerPassed = true;
                    }
                }
                // limit RS to 20
                if (match.FirstPlayerRS > 20)
                {
                    match.FirstPlayerRS = 20;
                }
                if (match.SecondPlayerRS > 20)
                {
                    match.SecondPlayerRS = 20;
                }

                _dbContext.SaveChanges();
                res.Target = new CardVM { UserId = playerId };
                res.ResponseResultType = ResponseResultType.Passed;
                return res;
            }
            catch (Exception)
            {

            }
            return null;
        }
        public MatchVM GetMatchVM(long playerId, long matchId)
        {
            MatchVM matchVM = null;
            try
            {
                if (_dbContext.Matches.Where(m => !m.IsDeleted && (m.SecondPlayerId == playerId || m.FirstPlayerId == playerId)).Any())
                {
                    matchVM = GetMatch(matchId);
                }
            }
            catch (Exception)
            {
            }
            return matchVM;
        }
        List<CardVM> Draw(int number, long userId, long matchId, MatchVM matchVM)
        {
            var list = new List<CardVM>();
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var cards = _dbContext.Cards.Where(a =>
                        a.MatchId == matchId && a.UserId == userId && a.Location == CardLocation.Deck).ToList();
                    if (cards.Count > 0)
                    {

                        cards.Shuffle();
                        cards = cards.Take(number).ToList();
                        var i = 0;
                        var groundCardNumbers = matchVM.Cards.Where(a => a.Location == CardLocation.Hand && a.UserId == userId).Select(a => a.PositionNumber).OrderBy(a => a).ToList();
                        foreach (var card in cards)
                        {
                            card.Location = CardLocation.Hand;
                            var firstAvailablePos = 0;
                            if (groundCardNumbers is not null)
                            {
                                for (int j = 0; j < 5; j++)
                                {
                                    if (groundCardNumbers.Contains(firstAvailablePos))
                                    {
                                        firstAvailablePos++;
                                    }
                                    else { break; }
                                }
                                groundCardNumbers.Add(firstAvailablePos);
                            }
                            card.PositionNumber = firstAvailablePos;
                            list.Add(_mapper.Map<CardVM>(card));
                        }
                        _dbContext.SaveChanges();
                        transaction.Commit();
                        return list;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            return null;
        }
        private MatchVM GetMatch(long matchId)
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
                            case ResponseResultType.Drawed:
                                {
                                    //var card = _dbContext.Cards.Where(a => a.CardId == response.Actor.CardId).First();
                                    //card.Location = CardLocation.Hand;
                                    //card.PositionNumber = response.Actor.PositionNumber;
                                    //_dbContext.SaveChanges();
                                }
                                break;
                            case ResponseResultType.Played:
                                {
                                    if (response.Target.UserId == match.FirstPlayerId)
                                    {
                                        match.FirstPlayerRS -= Convert.ToDouble(response.Target.Variables.Where(a => a.VariableName == "RC").First().VariableValue);
                                    }
                                    else
                                    {
                                        match.SecondPlayerRS -= Convert.ToDouble(response.Target.Variables.Where(a => a.VariableName == "RC").First().VariableValue);
                                    }
                                    var card = _dbContext.Cards.Where(a => a.CardId == response.Target.CardId).First();
                                    card.Location = CardLocation.Ground;
                                    card.PositionNumber = response.Target.PositionNumber;
                                    //_dbContext.SaveChanges();
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
                                    //_dbContext.SaveChanges();
                                }
                                break;
                            case ResponseResultType.Damaged:
                            case ResponseResultType.Healed:
                            case ResponseResultType.VariableChanged:
                                {
                                    var variable = _dbContext.Variables.Where(a => a.VariableId == response.Target_Variable.VariableId).First();
                                    variable.VariableValue = response.Target_Variable.VariableValue;
                                    //_dbContext.SaveChanges();
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
                                        match.SecondPlayerMorale -= Convert.ToDouble(response.Target.Variables.Where(a => a.VariableName == "MP").First().VariableValue);
                                    }
                                    var card = _dbContext.Cards.Where(a => a.CardId == response.Target.CardId).First();
                                    card.Location = CardLocation.Graveyard;
                                    card.PositionNumber = 0;
                                    //_dbContext.SaveChanges();
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    //_dbContext.SaveChanges();
                    match.FirstPlayerMorale = match.FirstPlayerMorale < 0 ? 0 : match.FirstPlayerMorale;
                    match.FirstPlayerRS = match.FirstPlayerRS < 0 ? 0 : match.FirstPlayerRS;
                    match.SecondPlayerMorale = match.SecondPlayerMorale < 0 ? 0 : match.SecondPlayerMorale;
                    match.SecondPlayerRS = match.SecondPlayerRS < 0 ? 0 : match.SecondPlayerRS;
                    _dbContext.SaveChanges();
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


        public bool ClearBoard(long matchId)
        {
            try
            {
                //var cards = _dbContext.Cards.Where(a => a.MatchId == matchId);
                //foreach (var card in cards)
                //{
                //    card.Location = CardLocation.Deck;
                //    card.PositionNumber = 0;
                //}
                //var m = _dbContext.Matches.Where(a => !a.IsDeleted && a.MatchId == matchId).First();
                //m.FirstPlayerRS = 10;
                //m.SecondPlayerRS = 10;
                //m.SecondPlayerMorale = 20;
                //m.FirstPlayerMorale = 20;
                //m.FirstPlayerPassed = false;
                //m.SecondPlayerPassed = false;
                //m.Phase = GamePhase.PlayPhase;
                //_dbContext.SaveChanges();

                //var match = GetMatch(matchId);
                //Draw(5, match.FirstPlayerId, matchId, match);
                //Draw(5, match.SecondPlayerId, matchId, match);

                var m = _dbContext.Matches.Where(a => !a.IsDeleted && a.MatchId == matchId).First();
                StartMatch(m.FirstPlayerId, m.SecondPlayerId);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }
    }
}
