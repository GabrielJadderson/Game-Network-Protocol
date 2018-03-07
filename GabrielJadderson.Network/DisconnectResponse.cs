namespace GabrielJadderson.Network
{
    public enum DisconnectResponse : byte
    {
        NONE = 0,
        INVALID_VERSION = 1,
        BAD_SESSION_ID = 2,
        THROTTLED = 3,
        RECONNECT = 4,
        SERVER_BEING_UPDATED = 5,
        BANNED = 6,
    }
}