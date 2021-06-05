namespace MyProject.Controllers
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using WebApiClient;
    using WebApiClient.Attributes;
    using MyProject.Models;

    [TraceFilter] 
    public interface ICustomFieldsApiClient : IHttpApi
    {
        [HttpGet("/api/customfields")]
        Task<IEnumerable<Entity>> GetCustomEntitiesAsync();
        [HttpGet("/api/customfields/{entity}")]
        Task<Entity> GetCustomEntityAsync(string entity);

        [HttpGet("/api/customfields/{entity}/fields")]
        Task<IEnumerable<FieldObj>> GetCustomFieldsAsync(string entity);

    }
}