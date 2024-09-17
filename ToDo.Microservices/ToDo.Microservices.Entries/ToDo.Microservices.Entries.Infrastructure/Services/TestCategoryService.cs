using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.UseCases.Services;

namespace ToDo.Microservices.Entries.Infrastructure.Services
{
    public class TestCategoryService : ICategoryService
    {
        public Dictionary<Guid, List<Category>> _categories = new Dictionary<Guid, List<Category>>()
        {
            { Guid.Parse("77cd1257-f5b4-4b81-9265-1594f810edbe"), new List<Category>()
            {
                 new Category(Guid.Parse("0cc1efc3-b24f-4351-aefd-484bcc362ed8"), "Test user category 1"),
                 new Category(Guid.Parse("bde24447-bc48-4a63-a2f2-d7edfa3bafaa"), "Test user category 2"),
                 new Category(Guid.Parse("2bab02ee-d10f-4b4d-a6f8-80777b6dfa30"), "Test user category 3"),
                 new Category(Guid.Parse("4e9d8b48-9fe5-48bf-a277-4606df8e8faa"), "Test user category 4"),
                 new Category(Guid.Parse("836bcd75-2d21-44e9-ac85-3e149dfe70d3"), "Test user category 5"),
            }},

            { Guid.Parse("dbe76299-2864-4c57-b757-de583221e999"), new List<Category>()
            {
                 new Category(Guid.Parse("43b0758d-9496-4705-ab55-2744685c1319"), "Test super category 1"),
                 new Category(Guid.Parse("29a1b6ca-f1f1-457d-8886-3b016b8afe96"), "Test super category 2"),
                 new Category(Guid.Parse("59a75d1d-a508-464b-b0cd-5e953acfe34a"), "Test super category 3"),
            } }
        };

        private ClaimsPrincipal _pricipal;

        public TestCategoryService(IHttpContextAccessor accessor)
        {
            _pricipal = accessor.HttpContext.User;   
        }

        public async Task<Result<IEnumerable<Category>>> Get()
        {
            return Result<IEnumerable<Category>>.Successful(_categories[Guid.Parse(_pricipal.Claims.First(x => x.Type == "subject").Value)]);
        }

        public async Task<Result<Category>> Get(Guid categoryId)
        {
            return Result<Category>.Successful(_categories[Guid.Parse(_pricipal.Claims.First(x => x.Type == "subject").Value)]
                                                          .First(x => x.Id == categoryId));
        }
    }
}
