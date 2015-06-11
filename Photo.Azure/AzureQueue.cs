using Photo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photo.Core.Models;

namespace Photo.Azure
{
    public class AzureQueue : ICloudQueue
    {
        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler NewMessageArrived;

        public Task<MemeRequest> DequeueMessage()
        {
            throw new NotImplementedException();
        }

        public Task EnqueueMessage(MemeRequest message)
        {
            throw new NotImplementedException();
        }
    }
}
