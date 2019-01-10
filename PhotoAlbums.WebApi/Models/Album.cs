using System.Collections.Generic;

namespace PhotoAlbums.WebApi.Models
{
    public class Album
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<Photo> Photos { get; set; }

        public Album WithPhotos(IEnumerable<Photo> photos)
        {
            Photos = photos;
            return this;
        }
    }
}