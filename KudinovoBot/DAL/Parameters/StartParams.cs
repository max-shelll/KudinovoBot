using PRTelegramBot.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KudinovoBot.DAL.Parameters
{
    public class StartParams : CustomParameters
    {
        public StartParams() 
        {
            InitData();
        }

        public string Location { get; set; }

        public override void ClearData()
        {
            Location = string.Empty;
        }

        public override void InitData()
        {
            
        }
    }
}
