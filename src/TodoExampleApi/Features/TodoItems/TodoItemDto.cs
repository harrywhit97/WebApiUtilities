using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoExampleApi.Models;
using WebApiUtilities.Abstract;
using WebApiUtilities.Interfaces;

namespace TodoExampleApi.Controllers
{
    public class TodoItemDto : Dto<TodoItem, long>
    {
        public string Description { get; set; }
        public virtual long ListId { get; set; }
    }
}
