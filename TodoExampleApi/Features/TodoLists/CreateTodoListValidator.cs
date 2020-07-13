using FluentValidation;

namespace TodoExampleApi.Features.TodoLists
{
    public class CreateTodoListValidator : AbstractValidator<CreateTodoList>
    {
        public CreateTodoListValidator()
        {
            RuleFor(x => x.ListName)
                .Length(1, 20);
        }
    }
}
