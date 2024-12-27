using System;
using System.Collections.Generic;
using ToDo.Cache.Abstractions;
using ToDo.Microservices.Categories.Domain.Models;

namespace ToDo.Microservices.Categories.UseCases.Caches
{
    public interface ICategoryCacheIO: ICacheIO<IEnumerable<Category>, Guid>
    {
    }
}
