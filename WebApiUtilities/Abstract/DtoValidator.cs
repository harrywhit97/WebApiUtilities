using FluentValidation;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.Abstract
{
    public abstract class DtoValidator<TDto, TBaseDto> : AbstractValidator<TDto>, IValidate<TBaseDto>
        where TBaseDto : IDto
        where TDto : TBaseDto
    {
    }
}
