using FluentValidation;
using TodoExampleApi.Models;
using WebApiUtilities.CrudRequests;

namespace TodoExampleApi.Features.TodoLists
{
    public class UpdateTodoListValidator : AbstractValidator<UpdateEntity<TodoList, long, TodoListDto>>
    {
        public UpdateTodoListValidator()
        {
            RuleFor(x => x.Entity.ListName)
                .Length(1, 20);
        }
    }
}
