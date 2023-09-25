using Regna.Core.Models;
using Regna.VM;

namespace Regna.Core.IServices
{
    public interface IAssetService
    {

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
