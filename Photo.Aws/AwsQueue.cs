using Photo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photo.Core.Models;

namespace Photo.Aws
{
    public class AwsQueue : ICloudQueue
    {
        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Task DequeueMessage(Func<MemeRequest, Task> processRequest)
        {
            throw new NotImplementedException();
        }

        public Task EnqueueMessage(MemeRequest message)
        {
            throw new NotImplementedException();
        }
    }
}
