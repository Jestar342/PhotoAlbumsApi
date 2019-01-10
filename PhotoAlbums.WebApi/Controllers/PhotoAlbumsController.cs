using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhotoAlbums.WebApi.Models;
using PhotoAlbums.WebApi.Services;

namespace PhotoAlbums.WebApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class PhotoAlbumsController : ControllerBase
    {
        readonly ITypicodeApi api;

        public PhotoAlbumsController(ITypicodeApi api)
        {
            this.api = api;
        }

        // GET api/albums
        [HttpGet("albums")]
        public async Task<IEnumerable<Album>> Get(int? userId = null)
        {
            var getAlbumsTask = api.Albums(userId);
            var getPhotosTask = api.Photos();

            await Task.WhenAll(getAlbumsTask, getPhotosTask);

            var albums = await getAlbumsTask;
            var photos = await getPhotosTask;

            var grouped = photos.ToLookup(p => p.AlbumId);

            return albums.Select(album => album.WithPhotos(grouped[album.Id]));
        }
    }
}
