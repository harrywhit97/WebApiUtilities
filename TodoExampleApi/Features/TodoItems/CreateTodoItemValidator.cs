using FluentValidation;

namespace TodoExampleApi.Features.TodoItems
{
    public class CreateTodoItemValidator : AbstractValidator<CreateTodoItem>
    {
        public CreateTodoItemValidator()
        {
            RuleFor(x => x.Description)
                .Length(1, 20);
        }
    }
}
