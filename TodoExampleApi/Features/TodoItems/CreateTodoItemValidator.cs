using FluentValidation;
using TodoExampleApi.Models;
using WebApiUtilities.CrudRequests;

namespace TodoExampleApi.Features.TodoItems
{
    public class CreateTodoItemValidator : AbstractValidator<CreateEntity<TodoItem, TodoItemDto>>
    {
        public CreateTodoItemValidator()
        {
            RuleFor(x => x.Entity.Description)
                .Length(1, 20);
        }
    }
}
