using Photo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photo.Core.Models;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace Photo.Aws
{
    public class AwsTable : ICloudTable
    {
        private static DateTime WayFutureDate = new DateTime(2100, 12, 01);

        private readonly AmazonDynamoDBClient client;
        private readonly string tableName;

        public AwsTable(string tableName)
        {
            var credentialFactory = new AwsCredentialFactory();

            client = new AmazonDynamoDBClient(credentialFactory.Credentials(), RegionEndpoint.USWest2);
            this.tableName = tableName;
        }

        public async Task<Guid> Add(CompletedMeme value)
        {
            if (value.Id == Guid.Empty)
                value.Id = Guid.NewGuid();

            if (value.CreatedOn == default(DateTime))
                value.CreatedOn = DateTime.UtcNow;

            var dateOrder = WayFutureDate - value.CreatedOn;

            var itemDictiontary = new Dictionary<string, AttributeValue>();
            itemDictiontary.Add("Id", new AttributeValue(value.Id.ToString()));
            itemDictiontary.Add("CreatedOn", new AttributeValue(value.CreatedOn.ToString("o")));
            itemDictiontary.Add("Text", new AttributeValue(value.Text));
            itemDictiontary.Add("ImageUrl", new AttributeValue(value.ImageUrl));
            itemDictiontary.Add("DateOrder", new AttributeValue() { N = dateOrder.Ticks.ToString() });

            await client.PutItemAsync(new PutItemRequest()
            {
                Item = itemDictiontary,
                TableName = tableName
            });

            return value.Id;
        }

        public async Task<CompletedMeme> Get(Guid id)
        {
            var itemDictiontary = new Dictionary<string, AttributeValue>();

            itemDictiontary.Add("Id", new AttributeValue(id.ToString()));

            var response = await client.GetItemAsync(new GetItemRequest()
            {
                Key = itemDictiontary,
                TableName = tableName
            });

            return ConvertItemToCompletedMeme(response.Item);
        }

        private CompletedMeme ConvertItemToCompletedMeme(Dictionary<string, AttributeValue> item)
        {
            return new CompletedMeme()
            {
                Id = Guid.Parse(item["Id"].S),
                CreatedOn = DateTime.Parse(item["CreatedOn"].S).ToUniversalTime(),
                Text = item["Text"].S,
                ImageUrl = item["ImageUrl"].S
            };
        }

        public async Task<IEnumerable<CompletedMeme>> Latest(int skip = 0, int limit = 10)
        {
            if(skip != 0)
            {
                throw new ArgumentException("Operation doesn't support skip of anything but 0");
            }

            var request = new ScanRequest(tableName)
            {
                IndexName = "DateOrder-index",
                Limit = 10,

            };

            var response = await client.ScanAsync(request);

            return response.Items.Select(item => ConvertItemToCompletedMeme(item)).ToArray();
        }
    }
}
