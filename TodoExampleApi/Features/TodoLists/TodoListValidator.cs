﻿using FluentValidation;

namespace TodoExampleApi.Features.TodoLists
{
    public class TodoListValidator<TDto> : AbstractValidator<TDto>
        where TDto : TodoListDto
    {
        public TodoListValidator()
        {
            RuleFor(x => x.ListName)
                .NotNull()
                .Length(1, 20);
        }
    }
}
