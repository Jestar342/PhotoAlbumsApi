using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoAlbums.WebApi.Models;
using Refit;

namespace PhotoAlbums.WebApi.Services
{
    public interface ITypicodeApi
    {
        [Get("/photos")]
        Task<IEnumerable<Photo>> Photos();

        [Get("/albums")]
        Task<IEnumerable<Album>> Albums(int? userId = null);
    }
}
