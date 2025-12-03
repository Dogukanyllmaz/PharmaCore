using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IDrugService
    {
        IDataResult<List<Drug>> GetAll();
        IDataResult<List<Drug>> GetByUnitPrice(decimal min, decimal max);
        IDataResult<Drug> GetById(int drugId);
        IResult Add(Drug drug);
        IResult Update(Drug drug);
        IResult Delete(Drug drug);
    }
}
