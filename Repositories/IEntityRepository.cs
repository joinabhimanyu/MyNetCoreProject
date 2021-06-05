using System.Collections.Generic;
using MyProject.Models;

namespace MyProject.Repositories
{
    public interface IEntityRepository
    {
        void Add(Entity entity);
        IEnumerable<Entity> GetAll(int count);
        Entity Find(string entity);
        void Remove(string entity);
        void Update(Entity item);
    }
}