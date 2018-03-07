using System;
using System.Runtime.CompilerServices;


namespace GabrielJadderson.Network
{

    /// <summary>
    /// full credit to https://github.com/manhunterita for this.
    /// https://gist.github.com/manhunterita/5b0263f6a3e4fb839704e6a8b68c6746
    /// </summary>
    public partial class NetworkBuffer
    {
        private const float GrowingFactor = 1.5f;
        private byte[] m_buffer;
        private int m_bitsPointer;

        protected NetworkBuffer()
        {

        }

        public static NetworkBuffer Create(int size)
        {
            var buffer = new NetworkBuffer();
            buffer.m_buffer = new byte[size];

            return buffer;
        }

        public void Recycle()
        {
            m_bitsPointer = 0;
            m_buffer = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ResetPointer()
        {
            m_bitsPointer = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureBufferSpace(int additionalSpaceInBits)
        {
            var length = m_buffer.Length;
            if (m_bitsPointer + additionalSpaceInBits > length << 3)
            {
                var tmpBuffer = m_buffer;
                var newBuffer = new byte[((int)(length * GrowingFactor) + 1)];
                Buffer.BlockCopy(tmpBuffer, 0, newBuffer, 0, tmpBuffer.Length);
                m_buffer = newBuffer;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteByte(byte data)
        {
            EnsureBufferSpace(8);
            var index = m_bitsPointer >> 3;
            m_buffer[index] = data;
            m_bitsPointer += 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBits(byte data, int bitsAmount)
        {
            if (bitsAmount <= 0) return;

            if (bitsAmount > 8) throw new ArgumentException("The max allowed value is 8.", "bitsAmount");

            // Identify the position in our buffer: m_bitsPointer / 8
            // The division operation is expensive on CPU.
            // We use a right-shift operator that is faster,
            // it is the same as writing m_bitsPointer / 2^3
            var positionInByte = m_bitsPointer >> 3;

            // Identify the bit offset relative to positionInByte:
            // m_bitsPointer % 8;
            // The division operation is expensive on CPU.
            // For this reason we use a & operation instead.
            // The explanation is the following.
            // We have that 81 / 8 = 10 and 81 % 8 = 1.
            // Given our x = 81 => 0 1 0 1 0 0 0 1
            // Given our y = 7  => 0 0 0 0 0 1 1 1
            // We apply & oper. => 0 0 0 0 0 0 0 1
            // This is the result of x % 8, but faster.
            var bitsOffset = m_bitsPointer & 0x7;

            // The amount of free bits in the current byte.
            var freeBits = 8 - bitsOffset;

            // The amount of left bits after our write operation
            var leftBitsAfterWriting = freeBits - bitsAmount;

            // We want to strip off bits that should not be written.
            // So let's say that bitsAmount is 5 and we have this:
            // data = 0 1 0 1 0 1 0 0
            // We mask it with a 0xFF right-shifted by 8 - 5 = 3
            // mask = 1 1 1 1 1 1 1 1 >> 3 = 0 0 0 1 1 1 1 1
            // The result of (data & mask):
            // 0 0 0 1 0 1 0 0
            data = (byte)(data & (0xFF >> 8 - bitsAmount));

            EnsureBufferSpace(bitsAmount);

            // If we have atleast 0 left bits, we are ok: we don't
            // need to write in the next byte.
            if (leftBitsAfterWriting >= 0)
            {
                // So prepare the mask.
                // First mask has to identify old bits in the buffer:
                // mask_1 = 0xFF >> freeBits       = 0 0 0 0 0 0 0 1
                // Second mask has to identify free left bits:
                // mask_2 = 0xFF << (8 - leftBits) = 1 1 0 0 0 0 0 0
                // This assuming that freeBits = 1 and leftBits = 2,
                // we have:
                // mask = mask_1 | mask_2          = 1 1 0 0 0 0 0 1
                // Where 0s are, we have our 5 bits to write.
                var mask = (0xFF >> freeBits) | (0xFF << (8 - leftBitsAfterWriting));

                // Apply the mask to the current byte in our buffer:
                // current byte = 1 1 1 1 1 1 1 1
                // mask         = 1 1 0 0 0 0 0 1
                // masked byte  = 1 1 0 0 0 0 0 1
                // Write the data left-shifted of bitsOffset:
                // data = 0 0 0 1 0 1 0 0 << 1 = 0 0 1 0 1 0 0 0
                // result = 1 1 1 0 1 0 0 1
                m_buffer[positionInByte] = (byte)((m_buffer[positionInByte] & mask) | (data << bitsOffset));
            }
            else
            {
                // Apply the mask to the current byte in our buffer.
                // Shift our data to fit free bits.
                m_buffer[positionInByte] = (byte)((m_buffer[positionInByte] & (0xFF >> freeBits)) | (data << bitsOffset));

                // Same here. This time we work on the next byte in
                // our buffer. So we mask it and write the remaining
                // bits.
                m_buffer[positionInByte + 1] = (byte)((m_buffer[positionInByte + 1] & (0xFF << (bitsAmount - freeBits))) | (data >> freeBits));
            }

            m_bitsPointer += bitsAmount;
        }
    }
}