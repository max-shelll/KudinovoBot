using PRTelegramBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KudinovoBot.DAL.Parameters
{
    public class WorkParameters : CustomParameters
    {
        public WorkParameters()
        {
            InitData();
        }

        public Guid WorkId { get; set; }

        public override void ClearData()
        {
            WorkId = Guid.Empty;
        }

        public override void InitData()
        {

        }
    }
}
