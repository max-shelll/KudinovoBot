using PRTelegramBot.Attributes;
using PRTelegramBot.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KudinovoBot.DAL.InclineCallbackHeaders
{
    [InlineCommand]
    public enum GeolocationHeader
    {
        Accept = 1,
        Deny = 2,
    }
}
