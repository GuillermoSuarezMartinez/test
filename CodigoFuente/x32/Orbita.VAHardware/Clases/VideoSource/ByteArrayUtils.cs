//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aibañez
// Created          : 05-11-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Some array utilities
    /// </summary>
    internal class ByteArrayUtils
    {
        #region Método(s) estático(s) público(s)
        /// <summary>
        /// Check if the array contains needle on specified position
        /// </summary>
        /// <param name="array"></param>
        /// <param name="needle"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static bool Compare(byte[] array, byte[] needle, int startIndex)
        {
            int needleLen = needle.Length;
            // compare
            for (int i = 0, p = startIndex; i < needleLen; i++, p++)
            {
                if (array[p] != needle[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Find subarray in array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="needle"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int Find(byte[] array, byte[] needle, int startIndex, int count)
        {
            int needleLen = needle.Length;
            int index;

            while (count >= needleLen)
            {
                index = Array.IndexOf(array, needle[0], startIndex, count - needleLen + 1);

                if (index == -1)
                    return -1;

                int i, p;
                // check for needle
                for (i = 0, p = index; i < needleLen; i++, p++)
                {
                    if (array[p] != needle[i])
                    {
                        break;
                    }
                }

                if (i == needleLen)
                {
                    // found needle
                    return index;
                }

                count -= (index - startIndex + 1);
                startIndex = index + 1;
            }
            return -1;
        } 
        #endregion
    }
}
