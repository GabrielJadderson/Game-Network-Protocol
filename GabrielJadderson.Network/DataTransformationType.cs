using System;


namespace GabrielJadderson.Network
{
    [Flags]
    public enum DataTransformationType : byte
    {
        /// Adds 128 to the value when it is written, takes 128 from the value when it is read (also known as type-A).
        ADD = 0x01,

        /// Negates the value (also known as type-C).
        NEGATE = 0x02,

        /// No transformation is done.
        NONE = 0x04,

        /// Subtracts the value from 128 (also known as type-S).
        SUBTRACT = 0x08,
    }
}