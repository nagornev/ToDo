using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ToDo.Domain.Results;
using ToDo.Extensions.Validator;
using ToDo.Microservices.Categories.API.Contracts.Categories;
using ToDo.Microservices.Categories.Domain.Models;
using ToDo.Microservices.Categories.UseCases.Services;
using ToDo.Microservices.Middleware.Identities;

namespace ToDo.Microservices.Categories.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : Controller
    {
        private ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Identity(IdentityPermissions.User)]
        public async Task<IResult> Get()
        {
            Result<IEnumerable<Category>> categoriesResult = await _categoryService.GetCategories(User.GetId());

            return categoriesResult.Success ?
                    Results.Ok(categoriesResult) :
                    Results.BadRequest(categoriesResult);
        }


        [HttpGet]
        [Identity(IdentityPermissions.User)]
        [Route("{id}")]
        public async Task<IResult> Get(Guid id)
        {
            Result<Category> categoryResult = await _categoryService.GetCategory(User.GetId(),
                                                                                 id);

            return categoryResult.Success ?
                    Results.Ok(categoryResult) :
                    Results.BadRequest(categoryResult);
        }

        [HttpPost]
        [Identity(IdentityPermissions.User)]
        public async Task<IResult> Create([FromServices] IValidator<CategoriesContractCreate> validator,
                                          [FromBody] CategoriesContractCreate contract)
        {
            if (!validator.Validate(contract, out Result validatonResult))
                return Results.BadRequest(validatonResult);

            Result creationResult = await _categoryService.CreateCategory(User.GetId(),
                                                                          contract.Name);

            return creationResult.Success ?
                    Results.Ok(creationResult) :
                    Results.BadRequest(creationResult);
        }

        [HttpPut]
        [Identity(IdentityPermissions.User)]
        public async Task<IResult> Update([FromServices] IValidator<CategoriesContractUpdate> validator,
                                          [FromBody] CategoriesContractUpdate contract)
        {
            if (!validator.Validate(contract, out Result validatonResult))
                return Results.BadRequest(validatonResult);

            Result updateResult = await _categoryService.UpdateCategory(User.GetId(),
                                                                        contract.CategoryId,
                                                                        contract.Name);

            return updateResult.Success ?
                    Results.Ok(updateResult) :
                    Results.BadRequest(updateResult);
        }

        [HttpDelete]
        [Identity(IdentityPermissions.User)]
        public async Task<IResult> Delete([FromServices] IValidator<CategoriesContractDelete> validator,
                                          [FromBody] CategoriesContractDelete contract)
        {
            if (!validator.Validate(contract, out Result validatonResult))
                return Results.BadRequest(validatonResult);

            Result deleteResult = await _categoryService.DeleteCategory(User.GetId(),
                                                                        contract.CategoryId);

            return deleteResult.Success ?
                    Results.Ok(deleteResult) :
                    Results.BadRequest(deleteResult);
        }
    }
}
