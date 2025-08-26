﻿using Elearning.Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Elearning.Infrastructure.SpecificationsEvaluator
{
    public class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var query = inputQuery;

            if (specification.Criteria is not null)
                query = query.Where(specification.Criteria);

            if (specification.OrderBy is not null)
                query = query.OrderBy(specification.OrderBy);

            if (specification.OrderByDescending is not null)
                query = query.OrderByDescending(specification.OrderByDescending);

            if (specification.IsPagingEnabled)
                query = query.Skip(specification.Skip)
                    .Take(specification.Take);

            query = specification.Includes
                .Aggregate(query, (cur, include) => cur.Include(include));

            return query;
        }
    }
}
