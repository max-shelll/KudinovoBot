using Newtonsoft.Json;
using PRTelegramBot.Models.CallbackCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KudinovoBot.DAL.TCommand
{
    public class WorkTCommand : TCommandBase
    {
        [JsonProperty("1")]
        public Guid WorkId { get; set; }

        public WorkTCommand(Guid workId, int command = 0) : base(command)
        {
            WorkId = workId;
        }
    }
}
