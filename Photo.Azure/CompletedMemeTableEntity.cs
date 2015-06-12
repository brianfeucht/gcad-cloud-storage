using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photo.Core.Models;

namespace Photo.Azure
{
    public class CompletedMemeTableEntity : TableEntity
    {
        internal const string PartitionKeyValue = "Meme";

        public Guid Id { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedOn { get; set; }

        public CompletedMemeTableEntity(CompletedMeme value)
        {
            if (value.Id == Guid.Empty)
                value.Id = Guid.NewGuid();

            if (value.CreatedOn == default(DateTime))
                value.CreatedOn = DateTime.UtcNow;

            // TODO: Implement partitioning since Azure doesnt support secondary indexes
            PartitionKey = PartitionKeyValue;
            RowKey = value.Id.ToString();

            Id = value.Id;
            CreatedOn = value.CreatedOn;
            ImageUrl = value.ImageUrl;
            Text = value.Text;
        }

        public CompletedMemeTableEntity()
        {

        }

        public static explicit operator CompletedMeme(CompletedMemeTableEntity value)
        {
            return new CompletedMeme()
            {
                Id = value.Id,
                CreatedOn = value.CreatedOn,
                ImageUrl = value.ImageUrl,
                Text = value.Text
            };
        }
    }
}
