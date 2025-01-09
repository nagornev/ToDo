﻿using System;
using System.Collections.Generic;
using ToDo.Cache.Abstractions;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.UseCases.Caches
{
    public interface ICategoryCacheReader : ICacheReader<IEnumerable<Category>, Guid>
    {
    }
}
