using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspect.Autofac.Caching;
using Core.Aspect.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.Logging.Loggers;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class DrugManager : IDrugService
    {
        IDrugDal _drugDal;

        public DrugManager(IDrugDal drugDal)
        {
            _drugDal = drugDal;
        }
        [LoggingAspect(typeof(SerilogLogger))]
        [ValidationAspect(typeof(DrugValidator))]
        [CacheRemoveAspect("IDrugService.Get")]
        public IResult Add(Drug drug)
        {
            _drugDal.Add(drug);
            return new SuccessResult(Messages.DrugAdded);
        }
        [LoggingAspect(typeof(SerilogLogger))]
        [CacheRemoveAspect("IDrugService.Get")]
        public IResult Delete(Drug drug)
        {
            _drugDal.Delete(drug);
            return new SuccessResult(Messages.DrugDeleted);
        }
        [LoggingAspect(typeof(SerilogLogger))]
        public IDataResult<List<Drug>> GetAll()
        {
            return new SuccessDataResult<List<Drug>>(_drugDal.GetAll(), Messages.DrugListed);
        }

        [LoggingAspect(typeof(SerilogLogger))]
        [CacheAspect]
        public IDataResult<Drug> GetById(int drugId)
        {
            return new SuccessDataResult<Drug>(_drugDal.Get(d => d.Id == drugId));
        }

        [LoggingAspect(typeof(SerilogLogger))]
        public IDataResult<List<Drug>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Drug>>(_drugDal.GetAll(d => d.Price >= min && d.Price <= max));
        }

        [LoggingAspect(typeof(SerilogLogger))]
        [ValidationAspect(typeof(DrugValidator))]
        [CacheRemoveAspect("IDrugService.Get")]
        public IResult Update(Drug drug)
        {
            _drugDal.Update(drug);
            return new SuccessResult(Messages.DrugUpdated);
        }
    }
}
