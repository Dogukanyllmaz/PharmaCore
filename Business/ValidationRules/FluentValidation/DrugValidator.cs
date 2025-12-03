using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class DrugValidator : AbstractValidator<Drug>
    {
        public DrugValidator()
        {
            RuleFor(d => d.Name).NotEmpty();
            RuleFor(d => d.Name).MinimumLength(2);
            RuleFor(d => d.Description).NotEmpty();
            RuleFor(d => d.Description).MaximumLength(400);
            RuleFor(d => d.Price).GreaterThan(0);
        }
    }
}
