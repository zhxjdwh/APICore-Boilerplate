﻿using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Repositories;
using Unit.Tests.UnitOfWork.Infrastructure;

namespace Unit.Tests.UnitOfWork.RepositoryTests
{
    [TestFixture]
    public class GetPagedListRepositoryTests : BaseRepository
    {
        [Test]
        [Description("Gets a Paged list of blog where Title = QWERTY")]
        public void RepositoryGet_PagedList_ListOfBlogsByPredicate()
        {
            var result = BlogRepository.GetPagedList(predicate: x => x.Title == "QWERTY");
            Assert.That(result.Items.Count, Is.GreaterThan(0));
        }

        [Test]
        [Description("Gets a Paged list of blog where Title = QWERTY with a fixed pageSize")]
        public void RepositoryGet_PagedList_ListOfBlogsWithASetPageLimit()
        {
            var result = BlogRepository.GetPagedList(predicate: x => x.Title == "QWERTY",
                pageSize: 2);

            Assert.That(result.Items.Count, Is.EqualTo(2));
        }

        [Test]
        [Description("Gets a Paged list of blog where Hits < 7 select their posts")]
        public void RepositoryGet_PagedList_WhereHitsLessThan7SelectPosts()
        {
            var result = BlogRepository.GetPagedList(predicate:x => x.Hits < 7, selector:y => y.Posts);

            Assert.That(result.Items.Count, Is.GreaterThan(0));
            Assert.That(result.Items.FirstOrDefault().FirstOrDefault(), Is.TypeOf<Post>());
        }

        [Test]
        [Description("Gets a Paged list of blog where Title = QWERTY with a fixed pageSize starting at page 2")]
        public void RepositoryGet_PagedList_ListOfBlogsWithASetPageLimitSetToAFixedPage()
        {
            var result = BlogRepository.GetPagedList(
                pageSize: 3,
                pageIndex: 2);

            Assert.That(result.TotalPages, Is.EqualTo(4));
            Assert.That(result.PageIndex, Is.EqualTo(2));
            Assert.That(result.Items.Count, Is.EqualTo(3));
        }

        [Test]
        [Description("Gets a Paged list of blog where Title = QWERTY and including the Post child object")]
        public void RepositoryGet_PagedList_ListOfBlogsByPredicateIncludePosts()
        {
            var result = BlogRepository.GetPagedList(predicate: x => x.Title == "QWERTY",
            include:
            source => source.Include(t => t.Posts));

            Assert.That(result.Items.Count, Is.GreaterThan(0));
            Assert.That(result.Items.FirstOrDefault().Posts, Is.Not.Null);
        }

        [Test]
        [Description("Gets a Paged list of blog where Title = ASDF including the Child object Post orderd by Hits in desc order")]
        public void RepositoryGet_PagedList_ListOfBlogsAndPostsOrderdByTitleDesc()
        {
            var result = BlogRepository.GetPagedList(
                predicate: x => x.Title == "ASDF",
                include: source => source.Include(t => t.Posts),
                orderBy: blog => blog.OrderByDescending(x => x.Hits),
                pageSize: 5);

            Assert.That(result.Items.Count, Is.GreaterThan(0));
            Assert.That(result.Items.FirstOrDefault().Hits, Is.GreaterThan(0));
        }
    }
}
