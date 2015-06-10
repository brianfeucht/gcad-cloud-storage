using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.Core.Models
{
    public class MemeRequest
    {
        public Guid Id { get; internal set; }
        public string Text { get; internal set; }
    }
}
