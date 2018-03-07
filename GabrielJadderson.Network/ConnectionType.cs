namespace GabrielJadderson.Network
{
    public enum ConnectionType : byte
    {
        LOGIN_REQUEST = 14,
        UPDATE = 15,
        NEW_CONNECTION_LOGIN = 16,
        RECONNECTING_LOGIN = 18,
    }
}