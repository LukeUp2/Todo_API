using Microsoft.AspNetCore.Mvc;
using Todo_API.Data;
using Todo_API.Enums;
using Todo_API.Models;

namespace Todo_API.Controllers
{
    [Route("/todo-api/todos")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private static TodoDbContext _context;

        public TodoController(TodoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult GetAll(EnumOptions options = EnumOptions.All)
        {
            if (options == EnumOptions.All)
            {
                var todos = _context.Todos.Where(t => !t.IsDeleted).ToList();
                return Ok(todos);
            }
            else if (options == EnumOptions.Done)
            {
                var todos = _context.Todos.Where(t => t.Done && !t.IsDeleted).ToList();
                return Ok(todos);

            } else if (options == EnumOptions.NotDone)
            {
                var todos = _context.Todos.Where(t => !t.Done && !t.IsDeleted).ToList();
                return Ok(todos);
            }

            return BadRequest();
        }

        [HttpPost]
        public ActionResult Post(Todo todo)
        {
            var exists = _context.Todos.SingleOrDefault(t => t.Title == todo.Title);
            if (exists != null)
            {
                return BadRequest("Vc ja tem uma tarefa com esse titulo!");
            }
            _context.Todos.Add(todo);
            _context.SaveChanges();
            return Ok(todo);
        }

        [HttpPut("{id}")]
        public ActionResult Put(Guid id, Todo changes)
        {
            var todo = _context.Todos.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound("Tarefa não encontrada");
            }
            todo.Title = changes.Title == "" ? todo.Title : changes.Title;
            todo.Description = changes.Description == "" ? todo.Description : changes.Description;

            _context.Todos.Update(todo);
            _context.SaveChanges();
            return Ok(todo);
        }

        [HttpPut("{id}/toggle-done")]
        public ActionResult ToggleDone(Guid id)
        {
            var todo = _context.Todos.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound("Tarefa não encontrada");
            }
            todo.Done = !todo.Done;
            if (todo.Done)
            {
                todo.DoneAt = DateTime.Now;
            }
            else
            {
                todo.DoneAt = DateTime.MinValue;
            }
            _context.Todos.Update(todo);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete]
        public ActionResult Delete(Guid id)
        {
            var todo = _context.Todos.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound("Tarefa não encontrada");
            }
            todo.IsDeleted = true;
            _context.Todos.Update(todo);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
