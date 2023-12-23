using PRTelegramBot.Attributes;

namespace KudinovoBot.DAL.Headers
{
    [InlineCommand]
    public enum WorkTHeader
    {
        PreviousPage,
        Create,
        Edit,
        Remove,
        NextPage
    }
}
