using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using EmbPortal.Shared.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public static class MappingExtensions
    {
        public static async Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize)
        {
            var count = await queryable.CountAsync();
            var items = await queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<TDestination>(items, count, pageNumber, pageSize);
        }

        public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration)
            => queryable.ProjectTo<TDestination>(configuration).ToListAsync();
    }
}
