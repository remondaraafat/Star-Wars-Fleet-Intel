using Application.ChainHandler;
using Domain.Models;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class ValidationHandlerPolicy : PooledObjectPolicy<ValidationHandler>
    {
        private readonly IServiceProvider _provider;

        public ValidationHandlerPolicy(IServiceProvider provider)
        {
            _provider = provider;
        }

        public override ValidationHandler Create()
        {
            var validator = _provider.GetRequiredService<IValidator<Starship>>();
            return new ValidationHandler(validator);
        }

        public override bool Return(ValidationHandler obj) => true;
    }

}
