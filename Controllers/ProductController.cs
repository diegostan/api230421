using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Shop.Models;
using Shop.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Shop.Controllers
{
    [Route("v1/products")]
    public class ProductController:ControllerBase
    {
        //Metodo Get    
        [AllowAnonymous]    
        [Route("")]
        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get([FromServices]DataContext context)
            {
                var products = await context.Products.Include(x=>x.Category).AsNoTracking().ToListAsync();
                return products;
            }
        
        //Metodo get por categoria
        [AllowAnonymous]
        [Route("categories/{id:int}")]
        [HttpGet]        
        public async Task<ActionResult<List<Product>>> GetByCategory([FromServices]DataContext context, int id)
        {
            var products = await context
            .Products
            .Include(s => s.Category)
            .AsNoTracking()
            .Where(s => s.CategoryId == id)
            .ToListAsync();

            return products;
        }

        //Metodo post        
        [Authorize(Roles="Operate")]      
        [Route("")]
        [HttpPost]
        public async Task<ActionResult<List<Product>>> Post([FromBody]Product model,
         [FromServices]DataContext context)
        {
               if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            try
            {
            context.Products.Add(model);            
            await context.SaveChangesAsync();
            return  Ok(model);
                
            }
            catch (System.Exception ex)
            {
                return BadRequest(new {message ="Não foi possível criar a categoria "+ ex.Message});            
            }
        }

         //Metodo put
        [Authorize(Roles="Operate")]      
        [Route("{id:int}")]
        [HttpPut]
        public async Task<ActionResult<List<Product>>> Put(
            int id, [FromBody]Product model,
            [FromServices]DataContext context
            )
        {

            if (model.ID != id)
                return NotFound(new {message = "Produto não encontrado"});

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry<Product> (model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new {message="Esse produto já foi atualizado"});                
            }
            catch(Exception)
            {
                return BadRequest(new {message="Não foi possível atualizar o produto"});                
            }
            
        }        
       
        //Metodo delete
        [Authorize(Roles="Master")]      
        [Route("{id:int}")]
        [HttpDelete]
        public async Task<ActionResult<List<Product>>> Delete(
            int id,
            [FromServices]DataContext context
        )
        
        {
            var product = await context.Products.FirstOrDefaultAsync(s=>s.ID==id);
                if (product == null) return NotFound(new{message= "Produto não encontrada"});

            try
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return Ok(new {message= $"Produto {product.Title} removido com sucesso"});
            }
            catch (System.Exception)
            {
                return BadRequest(new {message="Não foi possivel remover o produto"});                        
            }
            
        }




            
    }
}