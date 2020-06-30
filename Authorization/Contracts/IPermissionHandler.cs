namespace Authorization.Contracts
{
    public interface IPermissionHandler
    {
        bool IsAdministrator(ulong id);
    }
}