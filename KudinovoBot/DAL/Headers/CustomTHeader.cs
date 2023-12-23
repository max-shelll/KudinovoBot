using PRTelegramBot.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KudinovoBot.DAL.Headers
{
    [InlineCommand]
    public enum CustomTHeader
    {
        [Description("Пример 1")]
        ExampleOne = 500,
        [Description("Пример 2")]
        ExampleTwo,
        [Description("Пример 3")]
        ExampleThree,
        [Description("Пример страниц")]
        CustomPageHeader,
        [Description("Пример страниц2")]
        CustomPageHeader2,
    }
}
