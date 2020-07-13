using FluentValidation;
using TodoExampleApi.Models;
using WebApiUtilities.CrudRequests;

namespace TodoExampleApi.Features.TodoItems
{
    public class UpdateTodoItemValidator : AbstractValidator<UpdateEntity<TodoItem, long, TodoItemDto>>
    {
        public UpdateTodoItemValidator()
        {
            RuleFor(x => x.Entity.Description)
                .Length(1, 20);
        }
    }
}
