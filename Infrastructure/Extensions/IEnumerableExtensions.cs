using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Extensions
{
    public static class IEnumerableExtensions
    {
        public static List<Entity> Paginate<Entity>(this IEnumerable<Entity> data, int pageNumber, int pageSize)
        {
            return data.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
