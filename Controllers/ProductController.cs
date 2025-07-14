namespace CQRSExample.Controllers
{
  using CQRSExample.Features.Products.Commands;
  using CQRSExample.Features.Products.Queries;
  using MediatR;
  using Microsoft.AspNetCore.Mvc;

  [ApiController]
  [Route("api/[controller]")]
  public class ProductController : ControllerBase
  {
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
      var query = new GetAllProductsQuery();
      var products = await _mediator.Send(query);
      return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
      var query = new GetProductByIdQuery(id);
      var product = await _mediator.Send(query);
      if (product == null)
      {
        return NotFound();
      }
      return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
    {
      var productId = await _mediator.Send(command);
      return CreatedAtAction(nameof(GetProductById), new { id = productId }, productId);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductCommand command)
    {
      if (id != command.Id)
      {
        return BadRequest();
      }
      var result = await _mediator.Send(command);
      if (!result)
      {
        return NotFound();
      }
      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
      var command = new DeleteProductCommand(id);
      var result = await _mediator.Send(command);
      if (!result)
      {
        return NotFound();
      }
      return NoContent();
    }
  }
}
