using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPICore.Models
{
    public class Mail
    {
        public int LogId { get; set; }
        public string LoggedInPsNo { get; set; }
        public int LoggedInUserRoleId { get; set; }
        public string CallingMethodName { get; set; }
        public string To { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Sender { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Status { get; set; }
        public Dictionary<string, Stream> Attachments { get; set; }

    }
}
