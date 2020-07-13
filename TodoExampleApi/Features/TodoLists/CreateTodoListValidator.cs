using FluentValidation;
using TodoExampleApi.Models;
using WebApiUtilities.CrudRequests;

namespace TodoExampleApi.Features.TodoLists
{
    public class CreateTodoListValidator : AbstractValidator<CreateEntity<TodoList, TodoListDto>>
    {
        public CreateTodoListValidator()
        {
            RuleFor(x => x.Entity.ListName)
                .Length(1, 20);
        }
    }
}
