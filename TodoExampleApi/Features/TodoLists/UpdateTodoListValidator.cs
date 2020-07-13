using FluentValidation;

namespace TodoExampleApi.Features.TodoLists
{
    public class UpdateTodoListValidator : AbstractValidator<UpdateTodoList>
    {
        public UpdateTodoListValidator()
        {
            RuleFor(x => x.ListName)
                .Length(1, 20);
        }
    }
}
