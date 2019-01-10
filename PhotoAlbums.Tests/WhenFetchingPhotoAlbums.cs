using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using NUnit.Framework;
using PhotoAlbums.WebApi.Controllers;
using PhotoAlbums.WebApi.Models;
using PhotoAlbums.WebApi.Services;

namespace PhotoAlbums.Tests
{
    public class WhenFetchingPhotoAlbums
    {
        /*
            A note on member ordering and general test file layout:
        
            I tend to keep the context (SetUp) method, followed by test methods at the top
            as these are the "interesting" bits, which I want to be seen first and together.

            The context is both the setup ("Arrange") and the execution of the thing under test ("Act"), 
            then each test method is asserting a different attribute of the outcome.
         */

        [OneTimeSetUp]
        public void Setup()
        {
            SetupAlbumsAndPhotos();

            var api = FakeApi();
            InstantiateFetcher(api.FakedObject);

            FetchAlbums();
        }

        [Test]
        public void ShouldReturnCollectionWithSameLengthAsAlbums()
        {
            Assert.That(fetchedAlbums.Count(), Is.EqualTo(2));
        }

        [Test]
        public void FirstAlbumShouldHaveFivePhotos()
        {
            Assert.That(fetchedAlbums.Single(x => x.Id == 1).Photos.Count(), Is.EqualTo(5));
        }

        [Test]
        public void YourAlbumShouldHaveZeroPhotos()
        {
            Assert.That(fetchedAlbums.Single(x => x.Id == 2).Photos.Count(), Is.Zero);
        }

        void InstantiateFetcher(ITypicodeApi api)
        {
            controller = new PhotoAlbumsController(api);
        }

        void FetchAlbums()
        {
            /*
                I usually strive to avoid calling Wait()/GetAwaiter().GetResult() but NUnit doesn't support async setup
                and in the interests of keeping this exercise simple, I haven't included the synchronised synchronisation context
                polyfill. 
             */ 
            fetchedAlbums = controller.Get().GetAwaiter().GetResult();
        }

        Fake<ITypicodeApi> FakeApi()
        {
            var api = new Fake<ITypicodeApi>();
            api
                .CallsTo(x => x.Albums(null))
                .WithAnyArguments()
                // I would normally factor this into a separate test class specifically for testing the per-user search, 
                // to avoid any kind of logic in mocks as that is a false pass just waiting to happen.
                .ReturnsLazily((int? userId) => userId.HasValue ? albums.Where(x => x.UserId == userId.Value) : albums);

            api
                .CallsTo(x => x.Photos())
                .Returns(photos);

            return api;
        }

        void SetupAlbumsAndPhotos()
        {
            albums = new[]
                        {
                new Album
                {
                    Id = 1,
                    UserId = 1,
                    Title = "My album"
                },
                new Album
                {
                    Id = 2,
                    UserId = 2,
                    Title = "Your album"
                }
            };

            photos = new[]
            {
                new Photo
                {
                    AlbumId = 1,
                    Title = "First photo",
                    Url = new Uri("https://via.placeholder.com/600/92c952"),
                    ThumbnailUrl = new Uri("https://via.placeholder.com/150/92c952")
                },
                new Photo
                {
                    AlbumId = 1,
                    Title = "Second photo",
                    Url = new Uri("https://via.placeholder.com/600/92c952"),
                    ThumbnailUrl = new Uri("https://via.placeholder.com/150/92c952")
                },
                new Photo
                {
                    AlbumId = 1,
                    Title = "Third photo",
                    Url = new Uri("https://via.placeholder.com/600/92c952"),
                    ThumbnailUrl = new Uri("https://via.placeholder.com/150/92c952")
                },
                new Photo
                {
                    AlbumId = 1,
                    Title = "Fourth photo",
                    Url = new Uri("https://via.placeholder.com/600/92c952"),
                    ThumbnailUrl = new Uri("https://via.placeholder.com/150/92c952")
                },
                new Photo
                {
                    AlbumId = 1,
                    Title = "Fifth photo",
                    Url = new Uri("https://via.placeholder.com/600/92c952"),
                    ThumbnailUrl = new Uri("https://via.placeholder.com/150/92c952")
                },
            };
        }

        PhotoAlbumsController controller;
        IEnumerable<Album> fetchedAlbums;
        IEnumerable<Album> albums;
        IEnumerable<Photo> photos;
    }
}