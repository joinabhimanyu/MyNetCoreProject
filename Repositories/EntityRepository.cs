using System;
using System.Collections.Generic;
using System.Linq;
using MyProject.Models;

namespace MyProject.Repositories
{
    public class EntityRepository : IEntityRepository
    {
        private readonly EntityContext _context;

        public EntityRepository(EntityContext context)
        {
            _context = context;
            // Add(new TodoItem { Name = "Item1" });
        }

        public IEnumerable<Entity> GetAll(int count)
        {
            return _context.Entities.Where(e=>e.IsActive && !e.IsDeleted)
            .Select(e=>new Entity{EntityName=e.EntityName,Fields=e.Fields.Where(x=>x.IsActive && !x.IsDeleted).ToList(),IsActive=e.IsActive
            ,IsDeleted=e.IsDeleted,Key=e.Key})
            .Take(count).ToList();
        }

        public void Add(Entity item)
        {
            _context.Entities.Add(item);
            _context.SaveChanges();
        }

        public Entity Find(string entity)
        {
            return _context.Entities.Where(t => t.EntityName == entity)
            .Select(e=>new Entity{EntityName=e.EntityName,Fields=e.Fields.Where(x=>x.IsActive && !x.IsDeleted).ToList(),IsActive=e.IsActive
            ,IsDeleted=e.IsDeleted,Key=e.Key}).FirstOrDefault();
        }

        public void Remove(string entity)
        {
            var item = _context.Entities.First(t => t.EntityName==entity);
            _context.Entities.Remove(item);
            _context.SaveChanges();
        }

        public void Update(Entity item)
        {
            _context.Entities.Update(item);
            _context.SaveChanges();
        }
    }
}