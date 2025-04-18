using BRD.WebStore.Catalog.Infrastructure.Dtos;
using BRD.WebStore.Catalog.Infrastructure.Entities;
using BRD.WebStore.Catalog.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BRD.WebStore.Catalog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private ICatalogDbContext _dbContext;

    public ProductsController(ICatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #region Commands

    [HttpPost("Create")]
    public async Task Add(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
    }

    #endregion

    #region Queries

    [HttpGet("GetById")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);

        var result = Ok(product);
        return result;
    } 

    #endregion
}
