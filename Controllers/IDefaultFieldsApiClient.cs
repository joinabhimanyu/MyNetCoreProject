namespace MyProject.Controllers
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using WebApiClient;
    using WebApiClient.Attributes;
    using MyProject.Models;

    [TraceFilter]
    public interface IDefaultFieldsApiClient : IHttpApi
    {
        [HttpGet("/api/defaultfields")]
        Task<IEnumerable<Entity>> GetDefaultEntitiesAsync();
        [HttpGet("/api/defaultfields/{entity}")]
        Task<Entity> GetDefaultEntityAsync(string entity);
        [HttpGet("/api/defaultfields/{entity}/fields")]
        Task<IEnumerable<FieldObj>> GetDefaultFieldsAsync(string entity);

    }
}