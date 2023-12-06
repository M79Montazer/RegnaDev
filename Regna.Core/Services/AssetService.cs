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

        #region GenericVariable region
        public List<GenericVariableVM> GetAllGenericVariables()
        {
            try
            {
                var list = _dbContext.GenericVariables.AsEnumerable();
                list = list.Where(a => !a.IsDeleted);

                var res = _mapper.Map<List<GenericVariable>, List<GenericVariableVM>>(list.ToList());
                return res;
            }
            catch (Exception)
            {
            }
            return null;
        }
        public List<GenericVariableVM> GetListOfGenericVariables(int startIndex, int pageSize, ref int totalRecordsCount)
        {
            try
            {
                var list = _dbContext.GenericVariables.AsEnumerable();
                list = list.Where(a => !a.IsDeleted);
                totalRecordsCount = list.Count();
                if (totalRecordsCount > pageSize)
                {
                    list = list.Skip(startIndex).Take(pageSize);
                }
                var res = _mapper.Map<List<GenericVariable>, List<GenericVariableVM>>(list.ToList());
                return res;
            }
            catch (Exception)
            {
            }
            return null;
        }
        public GenericVariableVM AddGenericVariable(GenericVariableVM GenericVariableVM)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var GenericVariable = _mapper.Map<GenericVariable>(GenericVariableVM);
                    GenericVariable.CreateDate = DateTime.Now;
                    GenericVariable.IsDeleted = false;
                    _dbContext.GenericVariables.Add(GenericVariable);
                    _dbContext.SaveChanges();

                    var ocardIds = _dbContext.OCards.Where(a => !a.IsDeleted).Select(a => a.OCardId).ToList();
                    var newOVars = new List<OVariable>();
                    foreach (int id in ocardIds)
                    {
                        var newOVar = new OVariable
                        {
                            CreateDate = DateTime.Now,
                            IsDeleted = false,
                            InitialValue = GenericVariable.DefaultValue,
                            IsGeneric = true,
                            OCardID = id,
                            OVariableName = GenericVariable.GenericVariableName,
                            VariableType = GenericVariable.VariableType,
                        };
                        newOVars.Add(newOVar);
                    }
                    _dbContext.OVariables.AddRange(newOVars);
                    _dbContext.SaveChanges();
                    transaction.Commit();
                    GenericVariableVM.GenericVariableId = GenericVariable.GenericVariableId;
                    return GenericVariableVM;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            return null;
        }
        public GenericVariableVM UpdateGenericVariable(GenericVariableVM GenericVariableVM)
        {
            try
            {
                var GenericVariable = _dbContext.GenericVariables.Where(a => a.GenericVariableId == GenericVariableVM.GenericVariableId && !a.IsDeleted).First();
                GenericVariable.GenericVariableName = GenericVariableVM.GenericVariableName;
                GenericVariable.ModifyDate = DateTime.Now;

                _dbContext.SaveChanges();
                return GenericVariableVM;
            }
            catch (Exception)
            {
            }
            return null;
        }
        public bool DeleteGenericVariable(long GenericVariableId)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var GenericVariable = _dbContext.GenericVariables.Where(a => a.GenericVariableId == GenericVariableId).First();
                    GenericVariable.IsDeleted = true;
                    GenericVariable.DeleteDate = DateTime.Now;
                    _dbContext.SaveChanges();

                    var ovars = _dbContext.OVariables.Where(a => !a.IsDeleted && a.OVariableName.Equals(GenericVariable.GenericVariableName)).ToList();
                    foreach (var item in ovars)
                    {
                        item.IsDeleted = true;
                    }
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
        public GenericVariableVM GetGenericVariableById(long GenericVariableId)
        {
            try
            {
                var list = _dbContext.GenericVariables.AsEnumerable();
                list = list.Where(a => !a.IsDeleted);

                var res = _mapper.Map<GenericVariable, GenericVariableVM>(list.First());
                return res;
            }
            catch (Exception)
            {
            }
            return null;
        }

        #endregion

        #region OVariable region
        public List<OVariableVM> GetAllOVariables(long OCardId = 0)
        {
            try
            {
                var list = _dbContext.OVariables.AsEnumerable();
                list = list.Where(a => !a.IsDeleted);

                if (OCardId != 0)
                {
                    list = list.Where(a => a.OCardID == OCardId);
                }

                var res = _mapper.Map<List<OVariable>, List<OVariableVM>>(list.ToList());
                return res;
            }
            catch (Exception)
            {
            }
            return null;
        }
        public List<OVariableVM> GetListOfOVariables(int startIndex, int pageSize, ref int totalRecordsCount, long OCardId = 0)
        {
            try
            {
                var list = _dbContext.OVariables.AsEnumerable();
                list = list.Where(a => !a.IsDeleted);

                if (OCardId != 0)
                {
                    list = list.Where(a => a.OCardID == OCardId);
                }

                totalRecordsCount = list.Count();
                if (totalRecordsCount > pageSize)
                {
                    list = list.Skip(startIndex).Take(pageSize);
                }
                var res = _mapper.Map<List<OVariable>, List<OVariableVM>>(list.ToList());
                return res;
            }
            catch (Exception)
            {
            }
            return null;
        }
        public OVariableVM AddOVariable(OVariableVM OVariableVM)
        {
            try
            {
                if (_dbContext.OVariables.Where(a=>a.OVariableName.Equals(OVariableVM.OVariableName) && !a.IsDeleted).Any())
                {
                    return null;
                }
                var OVariable = _mapper.Map<OVariable>(OVariableVM);
                OVariable.CreateDate = DateTime.Now;
                OVariable.IsDeleted = false;
                OVariable.IsGeneric = false;
                _dbContext.OVariables.Add(OVariable);
                _dbContext.SaveChanges();
                OVariableVM.OVariableId = OVariable.OVariableId;
                return OVariableVM;
            }
            catch (Exception)
            {
            }
            return null;
        }
        public OVariableVM UpdateOVariable(OVariableVM OVariableVM)
        {
            try
            {
                var OVariable = _dbContext.OVariables.Where(a => a.OVariableId == OVariableVM.OVariableId && !a.IsDeleted).First();
                OVariable.OVariableName = OVariableVM.OVariableName;
                OVariable.ModifyDate = DateTime.Now;

                _dbContext.SaveChanges();
                return OVariableVM;
            }
            catch (Exception)
            {
            }
            return null;
        }
        public bool DeleteOVariable(long OVariableId)
        {
            try
            {
                if (_dbContext.OVariables.Where(a => a.OVariableId == OVariableId && a.IsGeneric).Any())
                {
                    return false;
                }
                var OVariable = _dbContext.OVariables.Where(a => a.OVariableId == OVariableId).First();
                OVariable.IsDeleted = true;
                OVariable.DeleteDate = DateTime.Now;

                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }
        public OVariableVM GetOVariableById(long OVariableId)
        {
            try
            {
                var list = _dbContext.OVariables.AsEnumerable();
                list = list.Where(a => !a.IsDeleted);

                var res = _mapper.Map<OVariable, OVariableVM>(list.First());
                return res;
            }
            catch (Exception)
            {
            }
            return null;
        }

        #endregion
        
        #region OCard region
        public List<OCardVM> GetAllOCards()
        {
            try
            {
                var list = _dbContext.OCards.AsEnumerable();
                list = list.Where(a => !a.IsDeleted);

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
                list = list.Where(a => !a.IsDeleted);
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
                var OCard = _dbContext.OCards.Where(a => a.OCardId == OCardVM.OCardId && !a.IsDeleted).First();
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
                list = list.Where(a => !a.IsDeleted);

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
