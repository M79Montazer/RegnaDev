using Regna.VM;
using Regna.VM.Enums;

namespace Regna.Core.Services
{
    public static class MechService
    {
        public static void RunMech(ResponseVM parentResponse, CardVM card, Listener listener, MatchVM matchVM,
            long? trigererId = null, long? targetId = null)
        {
            var response = new ResponseVM();
            try
            {
                foreach (var mech in card.Mechanics)
                {
                    //var flag = false;

                    /// conditions must be added here
                    if (mech.Listeners.Contains(listener))
                    {
                        mech.Events = mech.Events.OrderBy(a => a.Priority).ToList();
                        foreach (var e in mech.Events)
                        {
                            var baseCard = new CardVM();
                            switch (e.Base)
                            {
                                case Target.ChooseFromEnemy:
                                    break;
                                case Target.ChooseFromAlly:
                                    break;
                                case Target.Self:
                                    baseCard = card;
                                    break;
                                default:
                                    break;
                            }
                            string? baseAmount = "";
                            if (!string.IsNullOrEmpty(e.Base_VariableName))
                            {
                                baseAmount = card.Variables.Where(a => a.VariableName == e.Base_VariableName).First().VariableValue;
                            }
                            else
                            {
                                baseAmount = e.Amount;
                            }

                            var targetCard = new CardVM();
                            switch (e.Target)
                            {
                                case Target.ChooseFromEnemy:
                                    targetCard = matchVM.Cards.Where(a => a.CardId == targetId).First();
                                    break;
                                case Target.ChooseFromAlly:
                                    targetCard = matchVM.Cards.Where(a => a.CardId == targetId).First();
                                    break;
                                case Target.Self:
                                    targetCard = card;
                                    break;
                                default:
                                    break;
                            }
                            var v = targetCard.Variables.Where(a => a.VariableName.Equals(e.Target_VariableName)).First();
                            switch (e.ActionOPType)
                            {
                                case ActionOPType.Set:
                                    switch (v.VariableType)
                                    {
                                        case VariableType.number:
                                            v.VariableValue = baseAmount;
                                            break;
                                        case VariableType.unsigned:
                                            v.VariableValue = Convert.ToDouble(baseAmount) < 0 ? "0" : baseAmount;
                                            break;
                                        case VariableType.boolean:
                                            v.VariableValue = Convert.ToBoolean(baseAmount).ToString();
                                            break;
                                    }
                                    break;
                                case ActionOPType.Reduce:
                                    switch (v.VariableType)
                                    {
                                        case VariableType.number:
                                            v.VariableValue = (Convert.ToDouble(v.VariableValue) - Convert.ToDouble(baseAmount)).ToString();
                                            break;
                                        case VariableType.unsigned:
                                            v.VariableValue = (Convert.ToDouble(v.VariableValue) - Convert.ToDouble(baseAmount)).ToString();
                                            v.VariableValue = Convert.ToDouble(v.VariableValue) < 0 ? "0" : v.VariableValue;
                                            break;
                                        case VariableType.boolean:
                                            v.VariableValue = false.ToString().ToLower();
                                            break;
                                    }
                                    break;
                                case ActionOPType.Increase:
                                    switch (v.VariableType)
                                    {
                                        case VariableType.number:
                                            v.VariableValue = (Convert.ToDouble(v.VariableValue) + Convert.ToDouble(baseAmount)).ToString();
                                            break;
                                        case VariableType.unsigned:
                                            v.VariableValue = (Convert.ToDouble(v.VariableValue) + Convert.ToDouble(baseAmount)).ToString();
                                            v.VariableValue = Convert.ToDouble(v.VariableValue) < 0 ? "0" : v.VariableValue;
                                            break;
                                        case VariableType.boolean:
                                            v.VariableValue = true.ToString().ToLower();
                                            break;
                                    }

                                    break;
                                default:
                                    break;
                            }

                            response.Actor = card;
                            response.Target_Variable = v;
                            response.Target = targetCard;
                            response.ResponseResultType = e.ResponseResultType;
                            if (v.VariableName.Equals("HP") && Convert.ToDouble(v.VariableValue) <= 0)
                            {
                                response.ResponseResultType = ResponseResultType.Killed;
                            }
                            //response.
                            parentResponse.ChildResponses.Add(response);
                        }
                        //end of 
                    }
                }
            }
            catch (Exception)
            {

            }

            return;
        }

        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }
    }
}
