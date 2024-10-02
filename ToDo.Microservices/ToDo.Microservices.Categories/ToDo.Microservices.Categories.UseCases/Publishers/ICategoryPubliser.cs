using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDo.Domain.Results;

namespace ToDo.Microservices.Categories.UseCases.Publishers
{
    public interface ICategoryPubliser
    {
        Task<Result> Delete(Guid userId, Guid categoryId);
    }
}
