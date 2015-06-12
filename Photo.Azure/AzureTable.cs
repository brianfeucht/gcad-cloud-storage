using Photo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photo.Core.Models;
using Microsoft.WindowsAzure.Storage;
using System.Configuration;
using Microsoft.WindowsAzure.Storage.Table;

namespace Photo.Azure
{
    public class AzureTable : ICloudTable
    {
        private readonly string tableName;

        public CloudTable Table
        {
            get
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

                var client = storageAccount.CreateCloudTableClient();

                return client.GetTableReference(tableName);
            }
        }

        public AzureTable(string tableName)
        {
            this.tableName = tableName;
        }

        public async Task EnsureTableHasBeenCreated()
        {
            await Table.CreateIfNotExistsAsync();
        }

        public async Task<Guid> Add(CompletedMeme value)
        {
            var tableEntity = new CompletedMemeTableEntity(value);

            var operation = TableOperation.Insert(tableEntity);
            await Table.ExecuteAsync(operation);

            return tableEntity.Id;
        }

        public async Task<CompletedMeme> Get(Guid id)
        {
            var operation = TableOperation.Retrieve<CompletedMemeTableEntity>(CompletedMemeTableEntity.PartitionKeyValue, id.ToString());
            var result = await Table.ExecuteAsync(operation);
            return (CompletedMeme)(CompletedMemeTableEntity)result.Result;
        }

        public Task<IEnumerable<CompletedMeme>> Latest(int skip = 0, int limit = 10)
        {
            if (skip != 0)
                throw new ArgumentException("Skip is not supported");

            var query = Table.CreateQuery<CompletedMemeTableEntity>()
                .Where(e => e.PartitionKey == CompletedMemeTableEntity.PartitionKeyValue)
                // Order by not supported
                //.OrderByDescending(e => e.CreatedOn)
                // Skip is not supported
                //.Skip(0)
                .Take(limit);

            var results = query.ToArray();

            return Task.FromResult(results.Select(e => (CompletedMeme)e));
        }
    }
}
