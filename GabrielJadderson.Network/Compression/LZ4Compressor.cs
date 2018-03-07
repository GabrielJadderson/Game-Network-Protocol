/*
using System;
using System.IO;
using System.Linq;
using System.Text;
//using LZ4;
namespace GabrielJadderson.Network.Compression
{
    public class LZ4Compressor
    {

        public static string CompressStringAndSplit(string InputString, int splits)
        {
            try
            {
                return Convert.ToBase64String(LZ4Codec.Wrap(Encoding.UTF8.GetBytes(Enumerable.Repeat(InputString, splits).Aggregate((a, b) => a + "\n" + b))));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }

        public static string[] DecompressStringAndSplit(string InputString)
        {
            try
            {
                return Encoding.UTF8.GetString(LZ4Codec.Unwrap(Convert.FromBase64String(InputString))).Split('\n');

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }

        public static string CompressString(string InputString)
        {
            try
            {
                return Convert.ToBase64String(LZ4Codec.Wrap(Encoding.UTF8.GetBytes(InputString)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }

        public static string DecompressString(string InputString)
        {
            try
            {
                return Encoding.UTF8.GetString(LZ4Codec.Unwrap(Convert.FromBase64String(InputString)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }

        public static byte[] CompressBytes(byte[] bytes)
        {
            try
            {
                return LZ4Codec.Wrap(bytes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return null;
        }

        public static byte[] DecompressBytes(byte[] bytes)
        {
            try
            {
                return LZ4Codec.Unwrap(bytes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return null;
        }

        public static void CompressFile(string filePath, string contents)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                using (var lz4Stream = new LZ4Stream(fileStream, LZ4StreamMode.Compress))
                using (var writer = new StreamWriter(lz4Stream))
                {
                    writer.Write(contents);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static string DecompressFile(string filePath)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open))
                using (var lz4Stream = new LZ4Stream(fileStream, LZ4StreamMode.Decompress))
                using (var reader = new StreamReader(lz4Stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }

        public static void CompressFile(string filePath, byte[] contents)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                using (var lz4Stream = new LZ4Stream(fileStream, LZ4StreamMode.Compress))
                using (var writer = new BinaryWriter(lz4Stream))
                {
                    writer.Write(contents);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static byte[] DecompressFileBytes(string filePath)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open))
                using (var lz4Stream = new LZ4Stream(fileStream, LZ4StreamMode.Decompress))
                using (var reader = new BinaryReader(lz4Stream))
                {
                    return reader.ReadBytes((int)reader.BaseStream.Length);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }







    }
}
*/