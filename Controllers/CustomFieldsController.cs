using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyProject.Models;

namespace MyProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomFieldsController : ControllerBase, ICustomFieldsApiClient
    {
        private static readonly FieldObj CField1 = new FieldObj { Key = 5, Field = "CField1", IsRequired = true, MaxLength = 25, Source = "Source1", IsActive = true, IsDeleted = false };
        private static readonly FieldObj CField2 = new FieldObj { Key = 6, Field = "CField2", IsRequired = true, MaxLength = 25, Source = "Source2", IsActive = true, IsDeleted = false };
        private static readonly Entity Product1 = new Entity { Key = 1, EntityName = "Product1", Fields = new FieldObj[] { CField1, CField2 }, IsDeleted = false, IsActive = true };
        private static readonly FieldObj CField3 = new FieldObj { Key = 7, Field = "CField3", IsRequired = true, MaxLength = 25, Source = "Source1", IsActive = true, IsDeleted = false };
        private static readonly FieldObj CField4 = new FieldObj { Key = 8, Field = "CField4", IsRequired = true, MaxLength = 25, Source = "Source2", IsActive = true, IsDeleted = false };
        private static readonly Entity Product2 = new Entity { Key = 2, EntityName = "Product2", Fields = new FieldObj[] { CField3, CField4 }, IsDeleted = false, IsActive = true };
        private static readonly Entity[] Entities = new Entity[] { Product1, Product2 };
        public void Dispose() { }

        private readonly ILogger<CustomFieldsController> _logger;

        public CustomFieldsController(ILogger<CustomFieldsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public Task<IEnumerable<Entity>> GetCustomEntitiesAsync()
        {
            var rng = new Random();
            return Task.FromResult(Entities.Where(e => e.IsActive && !e.IsDeleted).Take(10));
        }
        [HttpGet]
        [Route("{entity}")]
        public Task<Entity> GetCustomEntityAsync(string entity)
        {
            if (Entities.Length > 0)
            {
                var item = Entities.Where(e => e.EntityName.ToLower().Trim() == entity.ToLower().Trim())
                .Where(e => e.IsActive && !e.IsDeleted)
                .Select(e => new Entity { EntityName = e.EntityName, IsActive = e.IsActive, IsDeleted = e.IsDeleted })
                .ToList();
                if (item.Count > 0)
                {
                    var fields = Entities.Where(e => e.EntityName.ToLower().Trim() == entity.ToLower().Trim())
                .Where(e => e.IsActive && !e.IsDeleted)
                .Select(e => e.Fields).ToList();
                    item.FirstOrDefault().Fields = fields.FirstOrDefault().Where(f => f.IsActive && !f.IsDeleted);
                }
                return Task.FromResult(item.FirstOrDefault());
            }
            return null;
        }
        [HttpGet]
        [Route("{entity}/fields")]
        public Task<IEnumerable<FieldObj>> GetCustomFieldsAsync(string entity)
        {
            if (Entities.Length > 0)
            {
                var fields = Entities.Where(e => e.EntityName.ToLower().Trim() == entity.ToLower().Trim())
                .Where(e => e.IsActive && !e.IsDeleted)
                .Select(e => e.Fields).ToList();
                return Task.FromResult(fields.FirstOrDefault().Where(f => f.IsActive && !f.IsDeleted));
            }
            return null;
        }
    }
}
