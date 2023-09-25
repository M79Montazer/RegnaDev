using AutoMapper;
using Regna.Core.Context;
using Regna.Core.IServices;
using Regna.Core.Models;
using Regna.VM;

namespace Regna.Core.Services
{
    public class AssetService : IAssetService
    {

        private readonly RegnaContext _dbContext;
        private readonly IMapper _mapper;
        public AssetService(RegnaContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        #region OCard region
        public List<OCardVM> GetAllOCards()
        {
            try
            {
                var list = _dbContext.OCards.AsEnumerable();
                list = list.Where(a => !a.IsDeleted.Value);

                var res = _mapper.Map<List<OCard>, List<OCardVM>>(list.ToList());
                return res;
            }
            catch (Exception)
            {
            }
            return null;
        }
        public List<OCardVM> GetListOfOCards(int startIndex, int pageSize, ref int totalRecordsCount)
        {
            try
            {
                var list = _dbContext.OCards.AsEnumerable();
                list = list.Where(a => !a.IsDeleted.Value);
                totalRecordsCount = list.Count();
                if (totalRecordsCount > pageSize)
                {
                    list = list.Skip(startIndex).Take(pageSize);
                }
                var res = _mapper.Map<List<OCard>, List<OCardVM>>(list.ToList());
                return res;
            }
            catch (Exception)
            {
            }
            return null;
        }
        public OCardVM AddOCard(OCardVM OCardVM)
        {
            try
            {
                var OCard = _mapper.Map<OCard>(OCardVM);
                OCard.CreateDate = DateTime.Now;
                OCard.IsDeleted = false;
                _dbContext.OCards.Add(OCard);
                _dbContext.SaveChanges();
                OCardVM.OCardId = OCard.OCardId;
                return OCardVM;
            }
            catch (Exception)
            {
            }
            return null;
        }
        public OCardVM UpdateOCard(OCardVM OCardVM)
        {
            try
            {
                var OCard = _dbContext.OCards.Where(a => a.OCardId == OCardVM.OCardId && !a.IsDeleted.Value).First();
                OCard.OCardName = OCardVM.OCardName;
                OCard.ModifyDate = DateTime.Now;

                _dbContext.SaveChanges();
                return OCardVM;
            }
            catch (Exception)
            {
            }
            return null;
        }
        public bool DeleteOCard(long OCardId)
        {
            try
            {
                var OCard = _dbContext.OCards.Where(a => a.OCardId == OCardId).First();
                OCard.IsDeleted = true;
                OCard.DeleteDate = DateTime.Now;

                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }
        public OCardVM GetOCardById(long OCardId)
        {
            try
            {
                var list = _dbContext.OCards.AsEnumerable();
                list = list.Where(a => !a.IsDeleted.Value);

                var res = _mapper.Map<OCard, OCardVM>(list.First());
                return res;
            }
            catch (Exception)
            {
            }
            return null;
        }

        #endregion
    }
}
