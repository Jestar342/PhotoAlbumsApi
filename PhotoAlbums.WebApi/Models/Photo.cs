using System;

namespace PhotoAlbums.WebApi.Models
{
    public class Photo
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public Uri Url { get; set; }
        public Uri ThumbnailUrl { get; set; }
    }
}