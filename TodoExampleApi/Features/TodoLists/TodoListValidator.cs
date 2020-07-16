using FluentValidation;
using WebApiUtilities.Interfaces;

namespace TodoExampleApi.Features.TodoLists
{
    public class TodoListValidator<TDto> : AbstractValidator<TDto>, IValidate<TodoListDto>
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
