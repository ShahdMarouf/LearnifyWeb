using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class AiChatRequest
    {
        public string sessionId { get; set; }
        public string message { get; set; }
        public bool includeSources { get; set; } = true;
    }
}
