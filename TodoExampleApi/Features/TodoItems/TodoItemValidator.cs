using FluentValidation;
using WebApiUtilities.Abstract;
using WebApiUtilities.Interfaces;

namespace TodoExampleApi.Features.TodoItems
{
    public class TodoItemValidator<TDto> : DtoValidator<TDto, TodoItemDto>
        where TDto : TodoItemDto
    {
        public TodoItemValidator()
        {
            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty()
                .Length(1, 20);
        }
    }
}
