using System;


namespace GabrielJadderson.Network
{
    public class GameMessage
    {
        private readonly Int16 _opcode;
        private readonly int _size;
        private readonly MessageType _type;
        private readonly ByteMessage _payload;


        public GameMessage(Int16 opcode, MessageType type, ByteMessage payload)
        {
            _opcode = opcode;
            _size = 0;
            _type = type;
            _payload = payload;
        }

        public short Opcode => _opcode;

        public int Size => _size;

        public MessageType Type => _type;

        public ByteMessage Payload => _payload;
    }
}