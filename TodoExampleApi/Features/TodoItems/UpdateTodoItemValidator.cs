using FluentValidation;

namespace TodoExampleApi.Features.TodoItems
{
    public class UpdateTodoItemValidator : AbstractValidator<UpdateTodoItem>
    {
        public UpdateTodoItemValidator()
        {
            RuleFor(x => x.Description)
                .Length(1, 20);
        }
    }
}
