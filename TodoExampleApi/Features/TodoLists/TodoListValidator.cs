using FluentValidation;
using WebApiUtilities.Abstract;
using WebApiUtilities.Interfaces;

namespace TodoExampleApi.Features.TodoLists
{
    public class TodoListValidator<TDto> : DtoValidator<TDto, TodoListDto>
        where TDto : TodoListDto
    {
        public TodoListValidator()
        {
            RuleFor(x => x.ListName)
                .NotNull()
                .Length(1, 20);
        }
    }
}
