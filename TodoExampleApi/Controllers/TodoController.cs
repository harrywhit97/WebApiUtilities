using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoExampleApi.Models;
using WebApiUtilities.Abstract;

namespace TodoExampleApi.Controllers
{
    public class TodoController : AbstractController<TodoItem, long>
    {
    }
}
