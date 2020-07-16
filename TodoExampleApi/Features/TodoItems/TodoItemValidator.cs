using FluentValidation;
using WebApiUtilities.Interfaces;

namespace TodoExampleApi.Features.TodoItems
{
    public class TodoItemValidator<T> : AbstractValidator<T>, IValidate<TodoItemDto>
        where T : TodoItemDto
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
