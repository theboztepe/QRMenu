using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Interceptors;
using Core.Utilities.Messages;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace Core.Aspects.Autofac.Validation
{
    public class ValidationAspect : MethodInterception
    {
        private readonly System.Type _validatorType;
        public ValidationAspect(System.Type validatorType)
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
            {
                throw new System.Exception(AspectMessages.WrongValidationType);
            }

            _validatorType = validatorType;
        }
        protected override void OnBefore(IInvocation invocation)
        {
            IValidator validator = (IValidator)System.Activator.CreateInstance(_validatorType);
            System.Type entityType = _validatorType.BaseType.GetGenericArguments()[0];
            IEnumerable<object> entities = invocation.Arguments.Where(t => t.GetType() == entityType);
            foreach (object entity in entities)
            {
                ValidationTool.Validate(validator, entity);
            }
        }
    }
}