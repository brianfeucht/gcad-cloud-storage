using Photo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photo.Core.Models;
using Raven.Client.Embedded;
using Raven.Client;

namespace Photo.Core
{
    public class LocalTable : ICloudTable
    {
        private static object lockObject = new object();
        private static EmbeddableDocumentStore documentStore;

        public LocalTable(string localFilePath)
        {
            if(documentStore == null)
            {
                lock(lockObject)
                {
                    if(documentStore == null)
                    {
                        documentStore = new EmbeddableDocumentStore
                        {
                            DataDirectory = localFilePath
                        };

                        documentStore.Initialize();
                    }
                }
            }
        }

        public async Task<Guid> Add(CompletedMeme value)
        {
            if (value.Id == default(Guid))
                value.Id = Guid.NewGuid();

            if (value.CreatedOn == default(DateTime))
                value.CreatedOn = DateTime.UtcNow;

            using (var session = documentStore.OpenAsyncSession())
            {
                await session.StoreAsync(value);
                await session.SaveChangesAsync();
            }

            return value.Id;
        }

        public async Task<CompletedMeme> Get(Guid id)
        {
            using (var session = documentStore.OpenAsyncSession())
            {
                return await session.LoadAsync<CompletedMeme>(id);
            }
        }

        public async Task<IEnumerable<CompletedMeme>> Latest(int skip = 0, int limit = 10)
        {
            using (var session = documentStore.OpenAsyncSession())
            {
                return await session.Query<CompletedMeme>()
                    .Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(5)))
                    .OrderByDescending(m => m.CreatedOn)
                    .Skip(skip)
                    .Take(limit)
                    .ToListAsync();
            }
        }
    }
}
