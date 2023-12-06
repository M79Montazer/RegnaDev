using AutoMapper;
using Regna.VM;

namespace Regna.Core.Models
{
    public class MapperProfiler : Profile
    {
        public MapperProfiler()
        {
            CreateMap<CardVM, Card>();
            CreateMap<Card, CardVM>();

            CreateMap<ConditionVM, Condition>();
            CreateMap<Condition, ConditionVM>();

            CreateMap<EventVM, Event>();
            CreateMap<Event, EventVM>();

            CreateMap<ListenerMechVM, ListenerMech>();
            CreateMap<ListenerMech, ListenerMechVM>();

            CreateMap<MechanicVM, Mechanic>();
            CreateMap<Mechanic, MechanicVM>();

            CreateMap<OCardVM, OCard>();
            CreateMap<OCard, OCardVM>();

            CreateMap<OVariableVM, OVariable>();
            CreateMap<OVariable, OVariableVM>();

            CreateMap<UserVM, User>();
            CreateMap<User, UserVM>();

            CreateMap<CardInDeckVM, CardInDeck>();
            CreateMap<CardInDeck, CardInDeckVM>();

            CreateMap<MechOCardVM, MechOCard>();
            CreateMap<MechOCard, MechOCardVM>();

            CreateMap<VariableVM, Variable>();
            CreateMap<Variable, VariableVM>();

            CreateMap<MatchVM, Match>();
            CreateMap<Match, MatchVM>();

            CreateMap<GenericVariableVM, GenericVariable>();
            CreateMap<GenericVariable, GenericVariableVM>();

        }
    }
}
