using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class QuizSubmitRequest
    {
        public string sessionId { get; set; }
        public List<string> answers { get; set; }
    }
}
