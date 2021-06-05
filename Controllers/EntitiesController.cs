using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyProject.Models;
using MyProject.Repositories;

namespace MyProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntitiesController : ControllerBase
    {
        // private static readonly Entity[] Entities = new Entity[] { };

        private readonly ILogger<EntitiesController> _logger;
        private readonly IEntityRepository _repo;
        private readonly dynamic client;

        public EntitiesController(ILogger<EntitiesController> logger, IEntityRepository repo)
        {
            _logger = logger;
            _repo = repo;
            client = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:5001")
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        [Route("{entity}")]
        public async Task<IEnumerable<Entity>> Get(string entity)
        {

            var url1 = string.Format("/api/customfields/{0}", entity);
            var url2 = string.Format("/api/defaultfields/{0}", entity);
            var result1 = new Entity();
            var result2 = new Entity();
            var response1 = await client.GetAsync(url1);
            if (response1.IsSuccessStatusCode)
            {
                var stringResponse1 = await response1.Content.ReadAsStringAsync();

                result1 = JsonSerializer.Deserialize<Entity>(stringResponse1,
                    new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var response2 = await client.GetAsync(url2);
                if (response2.IsSuccessStatusCode)
                {
                    var stringResponse2 = await response2.Content.ReadAsStringAsync();

                    result2 = JsonSerializer.Deserialize<Entity>(stringResponse2,
                        new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                    Entity eresult = null;
                    if (result1 != null && result2 != null)
                    {
                        eresult = new Entity { Key = result1.Key, EntityName = result1.EntityName, IsActive = result1.IsActive, IsDeleted = result1.IsDeleted };
                        List<FieldObj> fields = result1.Fields.Concat(result2.Fields).ToList();
                        for (int i = 0; i < fields.Count; i++)
                        {
                            fields[i].Key = i + 1;
                        }
                        eresult.Fields = fields;
                        var entityExists = _repo.Find(eresult.EntityName);
                        if (entityExists != null)
                        {
                            var efields = entityExists.Fields.ToList();
                            foreach (var item in fields)
                            {
                                if (efields.Where(x => x.Field == item.Field).ToList().FirstOrDefault() != null)
                                {
                                    efields.Where(x => x.Field == item.Field).ToList().FirstOrDefault().IsRequired = item.IsRequired;
                                    efields.Where(x => x.Field == item.Field).ToList().FirstOrDefault().MaxLength = item.MaxLength;
                                    efields.Where(x => x.Field == item.Field).ToList().FirstOrDefault().Source = item.Source;
                                    efields.Where(x => x.Field == item.Field).ToList().FirstOrDefault().IsActive = item.IsActive;
                                    efields.Where(x => x.Field == item.Field).ToList().FirstOrDefault().IsDeleted = item.IsDeleted;
                                }
                                else
                                {
                                    efields.Append(item);
                                }
                            }
                            for (int i = 0; i < efields.Count; i++)
                            {
                                efields[i].Key = i + 1;
                            }
                            eresult.Fields = efields;
                            eresult.Key=entityExists.Key;
                            _repo.Update(eresult);
                        }
                        else
                        {
                            _repo.Add(eresult);
                        }

                        return _repo.GetAll(10);
                    }
                }
                else
                {
                    throw new HttpRequestException(response2.ReasonPhrase);
                }
            }
            else
            {
                throw new HttpRequestException(response1.ReasonPhrase);
            }

            return null;
        }
    }
}
