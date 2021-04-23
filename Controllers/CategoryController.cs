using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Shop.Models;
using Shop.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

//Endpoint => URL
//HTTP://localhost:5000
//HTTPS://localhost:5001
//HTTPS://meuapp.azurewebsites.net/

namespace Shop.Controllers
{
    
    [Route("v1/categories")]
    public class CategoryController:ControllerBase
    {
        //Metodo get
        [AllowAnonymous]
        [Route("")]
        [HttpGet]
        public async Task<ActionResult<List<Category>>> Get(
            [FromServices]DataContext context
        )
        {
            var categories = await context.Categories.AsNoTracking().ToListAsync();
            return Ok(categories);
        }
        
        //Metodo get por id
        [AllowAnonymous]
        [HttpGet]
        [Route("{id:int}")]        
        public async Task<ActionResult<Category>> GetById(
            int id,
            [FromServices]DataContext context
            )
        {
            var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(s=>s.ID==id);
            return Ok(category);
        }       

        //Metodo post
        [Authorize(Roles="Operate")]
        [Route("")]
        [HttpPost]
        public async Task<ActionResult<List<Category>>> Post([FromBody]Category model, 
        [FromServices]DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            try
            {
            context.Categories.Add(model);            
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
        public async Task<ActionResult<List<Category>>> Put(
            int id, [FromBody]Category model,
            [FromServices]DataContext context
            )
        {

            if (model.ID != id)
                return NotFound(new {message = "Categoria não encontrada"});

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry<Category> (model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new {message="Esse registro já foi atualizado"});                
            }
            catch(Exception)
            {
                return BadRequest(new {message="Não foi possível atualizar a categoria"});                
            }
            
        }        
       
        //Metodo delete
        [Authorize(Roles="Master")]
        [Route("{id:int}")]
        [HttpDelete]
        public async Task<ActionResult<List<Category>>> Delete(
            int id,
            [FromServices]DataContext context
        )
        
        {
            var category = await context.Categories.FirstOrDefaultAsync(s=>s.ID==id);
                if (category == null) return NotFound(new{message= "Categoria não encontrada"});

            try
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return Ok(new {message= $"Categoria {id} removida com sucesso"});
            }
            catch (System.Exception)
            {
                return BadRequest(new {message="Não foi possivel remover a categoria"});                        
            }
            
        }

    }
}