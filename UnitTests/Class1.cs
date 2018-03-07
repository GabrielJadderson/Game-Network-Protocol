using System;

namespace UnitTests
{

    public class Class1
    {
        static void Main(string[] args)
        {


            byte[] bytes = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                bytes[i] = 1;
            }

            System.Diagnostics.Stopwatch s = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch s1 = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch s2 = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch s3 = new System.Diagnostics.Stopwatch();

            s.Start();
            for (int i = 0; i < 10000000; i++)
            {
                uint d2 = BitConverter.ToUInt32(bytes, 0);
            }
            s.Stop();


            s1.Start();
            for (int i = 0; i < 10000000; i++)
            {
                int d3 = BitConverter.ToInt32(bytes, 0);
            }
            s1.Stop();



            s2.Start();
            for (int i = 0; i < 10000000; i++)
            {
                int d4 = bytes[0] | (bytes[1] << 8) | (bytes[2] << 16) | (bytes[3] << 24);
            }
            s2.Stop();



            Console.WriteLine(s.ElapsedMilliseconds);
            Console.WriteLine(s.ElapsedTicks);
            Console.WriteLine();
            Console.WriteLine(s1.ElapsedMilliseconds);
            Console.WriteLine(s1.ElapsedTicks);
            Console.WriteLine();
            Console.WriteLine(s2.ElapsedMilliseconds);
            Console.WriteLine(s2.ElapsedTicks);


            Console.Read();
        }


        public static void PrintBits(byte[] b)
        {
            for (int i = b.Length - 1; i >= 0; i--)
            {
                Console.Write(Convert.ToString(b[i], 2).PadLeft(8, '0'));
                Console.Write(" ");
            }
            Console.WriteLine("");
        }

        public static byte[] longToBytes(long l)
        {
            byte[] result = new byte[8];
            for (int i = 7; i >= 0; i--)
            {
                result[i] = (byte)(l & 0xFF);
                l >>= 8;
            }
            return result;
        }

        public static long bytesToLong(byte[] b)
        {
            long result = 0;
            for (int i = 0; i < 8; i++)
            {
                result <<= 8;
                result |= (b[i] & 0xFF);
            }
            return result;
        }

        public static byte[] IntToBytes(int l)
        {
            byte[] result = new byte[8];
            for (int i = 3; i >= 0; i--)
            {
                result[i] = (byte)(l & 0xFF);
                l >>= 8;
            }
            return result;
        }

        public static int bytesToInt(byte[] b)
        {
            int result = 0;
            for (int i = 0; i < 4; i++)
            {
                result <<= 8;
                result |= (b[i] & 0xFF);
            }
            return result;
        }


        /*
        public class Loader
        {
            public static void Inject()
            {
                MessageBox.Show("Hello");
                Task.Run(() =>
                {
                    Thread.Sleep(3000);
                    MessageBox.Show("Hello again");
                    Thread.Sleep(5000);
                    MessageBox.Show("Hello again again");
                });
            }
        }
        */

    }

}