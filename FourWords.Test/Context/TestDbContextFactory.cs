using Microsoft.EntityFrameworkCore;
using FourWords.Server.Context;
using System;

namespace FourWords.Tests.Context
{
    public static class TestDbContextFactory
    {
        public static FourWordsDBContext Create()
        {
            var options = new DbContextOptionsBuilder<FourWordsDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new FourWordsDBContext(options);
            context.Database.EnsureCreated();

            return context;
        }

        public static void Destroy(FourWordsDBContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}