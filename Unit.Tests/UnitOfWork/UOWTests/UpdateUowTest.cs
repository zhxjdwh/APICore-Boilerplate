﻿using NUnit.Framework;
using NUnit.Framework.Internal;
using Repositories;
using TestObjects.ObjectMothers;
using Unit.Tests.UnitOfWork.Infrastructure;


namespace Unit.Tests.UnitOfWork.UOWTests
{
    [TestFixture]
    public class UpdateUowTest : BaseUnitOfWork
    {
        [Test]
        public void Uow_UpdateBlog_UpdatedBlog()
        {
            var blog = BlogObjectMother
                .aDefaultBlog()
                .ToRepository();

            Uow.GetRepository<Blog>().Insert(blog);
            Uow.SaveChanges();

            var insertResult = Uow.GetRepository<Blog>().Find(blog.Id);
            Assert.That(insertResult, Is.Not.Null);

            blog.Hits = 99;
            Uow.GetRepository<Blog>().Update(blog);
            var updateResult = Uow.GetRepository<Blog>().Find(blog.Id);
            Assert.That(updateResult.Hits, Is.EqualTo(99));
        }
    }
}
