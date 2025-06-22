using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using Products.Models;

[Route ("api/products")]
[ApiController]

public class ProductController : ControllerBase
{
    private static List<Item> items = new List<Item>
    {
        new Item { id = 1, name = "Laptop", quantity = 2 },
        new Item { id = 2, name = "Smartphone", quantity = 5 },
        new Item { id = 3, name = "Tablet", quantity = 3, description = "A lightweight tablet" },
    };

    [HttpGet]
    public ActionResult<List<Item>> GetAll()
    {
        return Ok(items);
    }

    [HttpGet("{id:int:range(1, 50)}")] // Example of advanced routing with constraints
    public ActionResult<Item> GetById(int id)
    {
        var item = items.FirstOrDefault(i => i.id == id);
        if (item == null)
        {
            return NotFound($"Item with ID {id} not found");
        }
        return Ok(item);
    }

    [HttpPost]
    public ActionResult<Item> AddItem(Item item){
        if (item == null) {
            return BadRequest("Item cannot be null");
        }
        items.Add(item);
        return Ok(item);
    }

    //ADD ADVANCED ROUTING - CLASS

    // like *path

    [HttpGet("disponibility")] // example of advanced routing with query parameters
    public ActionResult<Item> VerifyDisponibility([FromQuery] int id, [FromQuery] int quantity)
    {
        var item = items.FirstOrDefault(i => i.id == id);
        if (item == null)
        {
            return NotFound($"Item with ID {id} not found");
        }
        if (quantity > item.quantity)
        {
            return BadRequest($"Item {item.name} is not available in this quantity: {quantity}");
        }
        return Ok($"Item {item.name} is available in the requested quantity: {quantity}");
    }

    [HttpGet("browse/{category?}/{name?}/{*path}")] // Example of advanced routing with optional and catch-all parameters
    public IActionResult BrowseItems(string path, ProductCategory category = ProductCategory.All, string name = null)
    {
        return Ok($"You searched the Category: {category}, with item name: {name} and the server resources path for this case is: {path}");
    }
}