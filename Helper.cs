using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COD4SaveTool
{
    public static class Helper
    {
        /// <summary>
        /// Returns an int from a specific position in a byte array
        /// </summary>
        /// <param name="arr">Array to return int from </param>
        /// <param name="position">Position in the array to start reading</param>
        /// <returns>int stored at the specified position</returns>
        public static int ReadIntFromByteArray(byte[] arr, int position, bool littleEndian = true)
        {
            byte[] buffer = new byte[4];

            Buffer.BlockCopy(arr, position, buffer, 0, 4);

            if(!littleEndian)
            {
                Array.Reverse(buffer);
            }

            return BitConverter.ToInt32(buffer, 0);
        }

        public static void WriteIntToByteArray(byte[] arr, int position, int value, bool littleEndian = true)
        {
            byte[] buffer = BitConverter.GetBytes(value);

            if (!littleEndian)
            {
                Array.Reverse(buffer);
            }

            for(int i = 0; i < buffer.Length; i++)
            {
                Buffer.SetByte(arr, position, buffer[i]);
            }

        }

        public static void SetByteArrayInArray(byte[] src, int position, byte[] value)
        {
            for(int i = 0; i < value.Length; i++)
            {
                Buffer.SetByte(src, position + i, value[i]);
            }
        }


        //Greetz to StackOverflow
        // create a subset from a range of indices
        public static T[] RangeSubset<T>(this T[] array, int startIndex, int length)
        {
            T[] subset = new T[length];
            Array.Copy(array, startIndex, subset, 0, length);
            return subset;
        }

        // create a subset from a specific list of indices
        public static T[] Subset<T>(this T[] array, params int[] indices)
        {
            T[] subset = new T[indices.Length];
            for (int i = 0; i < indices.Length; i++)
            {
                subset[i] = array[indices[i]];
            }
            return subset;
        }

        //Greetz to Eamon Nerbonne
        public static IEnumerable<string> ChunksUpto(string str, int maxChunkSize)
        {
            for (int i = 0; i < str.Length; i += maxChunkSize)
                yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
        }
    }

    public static class Dbg
    {
        public static void ln(string str)
        {
            Console.WriteLine($"[DBG] : {str}");
        }
    }
}
