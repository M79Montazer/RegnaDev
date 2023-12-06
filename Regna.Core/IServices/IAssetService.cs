using Regna.Core.Models;
using Regna.VM;

namespace Regna.Core.IServices
{
    public interface IAssetService
    {

        #region GenericVariable region
        List<GenericVariableVM> GetAllGenericVariables();
        List<GenericVariableVM> GetListOfGenericVariables(int startIndex, int pageSize, ref int totalRecordsCount);
        GenericVariableVM AddGenericVariable(GenericVariableVM GenericVariableVM);
        GenericVariableVM UpdateGenericVariable(GenericVariableVM GenericVariableVM);
        bool DeleteGenericVariable(long GenericVariableId);
        GenericVariableVM GetGenericVariableById(long GenericVariableId);
        #endregion
        
        #region OVariable region
        List<OVariableVM> GetAllOVariables(long OCardId = 0);
        List<OVariableVM> GetListOfOVariables(int startIndex, int pageSize, ref int totalRecordsCount, long OCardId = 0);
        OVariableVM AddOVariable(OVariableVM OVariableVM);
        OVariableVM UpdateOVariable(OVariableVM OVariableVM);
        bool DeleteOVariable(long OVariableId);
        OVariableVM GetOVariableById(long OVariableId);
        #endregion
        
        #region OCard region
        List<OCardVM> GetAllOCards();
        List<OCardVM> GetListOfOCards(int startIndex, int pageSize, ref int totalRecordsCount);
        OCardVM AddOCard(OCardVM OCardVM);
        OCardVM UpdateOCard(OCardVM OCardVM);
        bool DeleteOCard(long OCardId);
        OCardVM GetOCardById(long OCardId);
        #endregion


    }
}
