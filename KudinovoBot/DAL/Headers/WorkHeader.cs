using PRTelegramBot.Attributes;

namespace KudinovoBot.DAL.Headers
{
    [InlineCommand]
    public enum WorkHeader
    {
        PreviousPage,
        Create,
        Edit,
        Remove,
        NextPage
    }
}
