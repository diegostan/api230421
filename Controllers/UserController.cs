using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Shop.Models;
using Shop.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Shop.Services;

namespace Shop.Controllers
{
    [Route("v1/user")]
    public class UserController:ControllerBase
    {
        //Metodo get
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<User>>> Get(
        [FromServices]DataContext context)
        {
            var user = await context.Users.AsNoTracking().ToListAsync();
            
            return Ok(user);
        }

        //Metodo get por id
        [AllowAnonymous]
        [Route("{id:int}")]
        [HttpGet]
        public async Task<ActionResult<User>> GetByID(
            [FromServices]DataContext context, int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(new {message= "Modelo invalido"});

            var user = await context.Users.AsNoTracking().Where(s => s.ID==id).FirstOrDefaultAsync();
            user.Password= null;
            return Ok(user);
        }

        //Metodo post
        [Authorize(Roles="Master")]
        [HttpPost]
        public async Task<ActionResult<User>> Post([FromBody]User model,
        [FromServices]DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(new{message="Usuário inválido"});

            try
            {
                context.Users.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (System.Exception)
            {
                return BadRequest(new {message= "Não foi possível incluir o usuário"});                
            }
        }

        //Metodo login
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromServices]DataContext context,
        [FromBody]User model)
        {
            var user = await context.Users.AsNoTracking()
            .Where(s => s.UserName == model.UserName && s.Password==model.Password)
            .FirstOrDefaultAsync();

            if(user==null)
                return NotFound(new {message="Usuário e/ou senha inválido"});

            var token = TokenService.GenerateToken(user);
            
            return new {
                user1=user,
                token1=token
            };
        }

        //Metodo put
        [Authorize(Roles="Master")]
        [Route("{id:int}")]
        [HttpPut]
        public async Task<ActionResult<User>> Put([FromBody]User model,
        [FromServices]DataContext context, int id)
        {
            if(model.ID!=id)
                return NotFound(new {message="Usuário não encontrado"});

            if (!ModelState.IsValid)
                return BadRequest(new {message="Dados de usuario invalidos"});

             try
             {
             context.Entry<User>(model).State = EntityState.Modified;
             await context.SaveChangesAsync();
             model.Password= null;
             return Ok(model);
             }
             catch
             {
                 return BadRequest(new {message="Falha ao atualizar usuário"});
             }
        }

        //Metodo delete
        [Authorize(Roles="Master")]
        [Route("{id:int}")]
        [HttpDelete]
        public async Task<ActionResult<User>> Delete(int id,
        [FromServices]DataContext context)
        {
            try
            {
            var user = await context.Users.AsNoTracking().Where(s => s.ID==id).FirstOrDefaultAsync();
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return Ok(new {message= $"Usuário {user.UserName} removido com sucesso"});
            }
            catch
            {
               return BadRequest(new {message="Não foi possível remover o usuário"});
            }
        }
        
    }
    
}