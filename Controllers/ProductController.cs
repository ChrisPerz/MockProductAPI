using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using Products.Models;

[Route ("api/products")]
[ApiController]

public class ProductController : ControllerBase
{

    private static Dictionary<int, Item> items = new Dictionary<int, Item> // Id is not related to the index of the list, so we use a dictionary for better performance
    {
        { 1, new Item { name = "Laptop", quantity = 2 } },
        { 2, new Item { name = "Smartphone", quantity = 5 } },
        { 3, new Item { name = "Tablet", quantity = 3, description = "A lightweight tablet" } },
    };

    [HttpGet]
    public ActionResult<List<Item>> GetAll()
    {
        return Ok(items);
    }

    [HttpGet("{id:int:range(1, 50)}")] // Example of advanced routing with constraints
    public ActionResult<Item> GetById(int id)
    {
        if (items.TryGetValue(id, out var item))
        {
            return Ok(item);
        }
        return NotFound($"Item with ID {id} not found");
    }

   [HttpPost]
    public ActionResult<Item> AddItem(Item item, int? itemId)
    {
        if (item == null)
        {
            return BadRequest("Item cannot be null");
        }

        if (itemId.HasValue)
        {
            if (items.ContainsKey(itemId.Value))
            {
                return Conflict($"Item with ID {itemId.Value} already exists");
            }

            items[itemId.Value] = item;
            return Ok(item);
        }

        var newId = items.Count > 0 ? items.Keys.Max() + 1 : 1;
        items[newId] = item;
        return Ok(item);
    }

    [HttpDelete("{id:int:range(1, 50)}")] // Example of advanced routing with constraints
    public ActionResult<Item> DeleteItem(int id)
    {
        if (items.TryGetValue(id, out var item))
        {
            items.Remove(id);
            return Ok(item);
        }
        return NotFound($"Item with ID {id} not found");
    }

    [HttpGet("disponibility")]
    public ActionResult<string> VerifyDisponibility([FromQuery] int id, [FromQuery] int quantity)
    {
        if (items.TryGetValue(id, out var item))
        {
            if (quantity > item.quantity)
            {
                return BadRequest($"Item {item.name} is not available in this quantity: {quantity}");
            }
            return Ok($"Item {item.name} is available in the requested quantity: {quantity}");
        }
        return NotFound($"Item with ID {id} not found");
    }

    [HttpGet("browse/{category}/{*path}")] // Example of advanced routing with optional and catch-all parameters
    public IActionResult GetCategoryResources(ProductCategory category, string path)
    {
        return Ok($"You searched the Category: {category}, and the server resources path for this case is: {path}");
    }
}