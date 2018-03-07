using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GabrielJadderson.Network.Serialization
{
    public class ObjectSerializer
    {

        private ObjectSerializer()
        {
            /*
            string fn = "test.dat";



            Hashtable test = new Hashtable();
            test.Add("Jeff", "123 Main Street, Redmond, WA 98052");
            test.Add("Fred", "987 Pine Road, Phila., PA 19116");
            test.Add("Mary", "PO Box 112233, Palo Alto, CA 94301");

            SerializeToFile(fn, test);



            Hashtable test2 = null;


            DeserializeFromFile(fn, ref test2);

            // To prove that the table deserialized correctly,
            // display the key/value pairs.
            foreach (DictionaryEntry de in test2)
            {
                Console.WriteLine("{0} lives at {1}.", de.Key, de.Value);
            }

            Console.WriteLine("");
            string s = "fuck your mom you lil shit.";

            byte[] serialzied = Serialize(ref s);
            Console.WriteLine(serialzied.Length);
            string outs = "";
            Deserialize(ref serialzied, ref outs);

            Console.WriteLine(outs);
            */
        }

        public static void SerializeToFile(string filePath, object contents)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    formatter.Serialize(fs, contents);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
            }
        }

        public static void DeserializeFromFile<T>(string filePath, ref T t)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    t = (T)new BinaryFormatter().Deserialize(fs);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
            }
        }

        public static byte[] Serialize<T>(ref T contents)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(ms, contents);
                    ms.Flush();
                    return ms.ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }

        public static void Deserialize<T>(ref byte[] serializedBytes, ref T contents)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(serializedBytes))
                {
                    contents = (T)new BinaryFormatter().Deserialize(ms);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


    }
}