using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ODATA.MS.CORE.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODATA.MS.CORE.API.Controllers
{
    class BaseApiController<T, S> : ODataController where T : BaseEntity<T> where S : ODataController
    {
        protected readonly DbContext _DbContext;
        protected readonly ILogger<S> _logger;
        private readonly DbSet<T> dbSet;
        public BaseApiController(ILogger<S> logger, DbContext DbContext)
        {
            _logger = logger;
            _DbContext = DbContext;
            dbSet = _DbContext.Set<T>();
        }

        [EnableQuery]
        public virtual IActionResult Get()
        {
            return Ok(_DbContext.Set<T>().AsQueryable<T>());
        }

        public virtual IActionResult Post([FromBody] T entity)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            dbSet.Add(entity);
            _DbContext.SaveChanges();
            return Created(entity);
        }


        public virtual IActionResult Put(int key, [FromBody] T entity)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            dbSet.Update(entity);
            _DbContext.SaveChanges();
            return Ok(entity);
        }

        public virtual IActionResult Delete(int key)
        {
            dbSet.Remove(dbSet.Find(key));
            _DbContext.SaveChanges();
            return NoContent();
        }
    }
}
