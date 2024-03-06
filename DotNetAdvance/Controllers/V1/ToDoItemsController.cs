using Asp.Versioning;
using DotNetAdvance.Interface;
using DotNetAdvance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DotNetAdvance.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiVersion("1.0")]
    public class ToDoItemsController : ControllerBase
    {
        private readonly ITodoItem _todoItemContext;


        public ToDoItemsController(ITodoItem todoItemContext)
        {
            _todoItemContext = todoItemContext;
        }

        // GET: api/ToDoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetToDoItems()
        {
            var list = await _todoItemContext.GetToDoItemsAsync();
            return Ok(list);
        }

        // GET: api/ToDoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItem>> GetToDoItem(int id)
        {
            var toDoItem = await _todoItemContext.GetToDoItemAsync(id);

            if (toDoItem == null)
            {
                Log.Error($"GetToDoItem : Item not found with id {id}");
                return NotFound();
            }

            return toDoItem;
        }

        // PUT: api/ToDoItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoItem(int id, ToDoItem toDoItem)
        {
            if (id != toDoItem.Id)
            {
                Log.Error($"UpdateToDoItem : Item Id {toDoItem.Id} and given id {id} mismatch");
                return BadRequest();
            }

            try
            {
                var res = await _todoItemContext.UpdateToDoItem(id, toDoItem);
                if (res >= 1)
                {
                    Log.Information($"UpdateToDoItem : Item with id {id} updated successfully");
                    return Ok("Updated Successfully");
                }
                else
                {
                    Log.Error($"UpdateToDoItem : Item with id {id} was not updated.");
                    return Problem("Could not be updated.");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                var item = await _todoItemContext.GetToDoItemAsync(id);
                if (item == null)
                {
                    Log.Error($"UpdateToDoItem : Item with id {id} generated DbCuncurrencyException");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/ToDoItems
        [HttpPost]
        public async Task<ActionResult<ToDoItem>> PostToDoItem(ToDoItem toDoItem)
        {
            if (!ModelState.IsValid)
            {
                Log.Error($"PostToDoItem : Item with id {toDoItem.Id} does not have complete details");
                return Problem("Deatils not found for todo item.");
            }
            var res = await _todoItemContext.AddToDoItem(toDoItem);
            if (res >= 1)
            {
                Log.Information($"PostToDoItem : Item with id {toDoItem.Id} added successfully");
                return Ok("Todo Item added successfully.");
            }
            Log.Error($"PostToDoItem : Item with id {toDoItem.Id} cannot not be added");
            return Problem("Cannot be added");
        }

        // DELETE: api/ToDoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItem(int id)
        {
            var toDoItem = await _todoItemContext.GetToDoItemAsync(id);
            if (toDoItem == null)
            {
                Log.Error($"DeleteToDoItem : Item with id {id} could not be found");
                return NotFound("The todo item cannot be found");
            }

            var res = await _todoItemContext.DeleteToDoItemAsync(toDoItem.Id);
            if (res >= 1)
            {
                Log.Information($"DeleteToDoItem : Item with id {id} deleted successfully.");
                return Ok("Todo Item deleted successfully.");
            }
            Log.Error($"DeleteToDoItem : Item with id {id} could not be deleted.");
            return Problem("Item cannot be deleted");
        }
    }
}
