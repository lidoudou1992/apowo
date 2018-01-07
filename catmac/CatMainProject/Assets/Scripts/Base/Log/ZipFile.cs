﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.IO;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Core;
public static class ZipExtension
{
    public static void Compress(FileInfo fileToCompress)
    {
        using (FileStream fs = File.OpenRead(fileToCompress.FullName))
        {
            if ((File.GetAttributes(fileToCompress.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
            {
                using (GZipOutputStream s = new GZipOutputStream(File.Create(fileToCompress.FullName + ".gz")))
                {
                    byte[] writeData = new byte[4096];
                    StreamUtils.Copy(fs, s, writeData);
                }
            }
        }
    }

    public static byte[] ZipStream(byte[] sBuffer)
    {

        MemoryStream m_msBZip2 = null;

        BZip2OutputStream m_osBZip2 = null;

        byte[] result;

        try
        {

            m_msBZip2 = new MemoryStream();

            Int32 size = sBuffer.Length;


            using (BinaryWriter writer = new BinaryWriter(m_msBZip2, System.Text.Encoding.ASCII))
            {

                writer.Write(size);



                m_osBZip2 = new BZip2OutputStream(m_msBZip2);

                m_osBZip2.Write(sBuffer, 0, sBuffer.Length);



                m_osBZip2.Close();

                result = m_msBZip2.ToArray();

                m_msBZip2.Close();



                writer.Close();

            }

        }

        finally
        {

            if (m_osBZip2 != null)
            {

                m_osBZip2.Dispose();

            }

            if (m_msBZip2 != null)
            {

                m_msBZip2.Dispose();

            }

        }

        return result;

    }
    public static byte[] ZipByte(byte[] sBuffer)
    {

        MemoryStream m_msBZip2 = null;

        BZip2OutputStream m_osBZip2 = null;

        byte[] result;

        try
        {

            m_msBZip2 = new MemoryStream();

            Int32 size = sBuffer.Length;

            // Prepend the compressed data with the length of the uncompressed data (firs 4 bytes)

            //

            using (BinaryWriter writer = new BinaryWriter(m_msBZip2, System.Text.Encoding.ASCII))
            {

                writer.Write(size);



                m_osBZip2 = new BZip2OutputStream(m_msBZip2);

                m_osBZip2.Write(sBuffer, 0, sBuffer.Length);



                m_osBZip2.Close();

                result = m_msBZip2.ToArray();

                m_msBZip2.Close();



                writer.Close();

            }

        }

        finally
        {

            if (m_osBZip2 != null)
            {

                m_osBZip2.Dispose();

            }

            if (m_msBZip2 != null)
            {

                m_msBZip2.Dispose();

            }

        }

        return result;

    }



    public static byte[] UnZipByte(byte[] compbytes)
    {

        byte[] bytesUncompressed;

        MemoryStream m_msBZip2 = null;

        BZip2InputStream m_isBZip2 = null;

        try
        {

            m_msBZip2 = new MemoryStream(compbytes);

            // read final uncompressed string size stored in first 4 bytes

            //

            using (BinaryReader reader = new BinaryReader(m_msBZip2, System.Text.Encoding.ASCII))
            {

                Int32 size = reader.ReadInt32();



                m_isBZip2 = new BZip2InputStream(m_msBZip2);

                bytesUncompressed = new byte[size];

                m_isBZip2.Read(bytesUncompressed, 0, bytesUncompressed.Length);

                m_isBZip2.Close();

                m_msBZip2.Close();


                reader.Close();

            }

        }

        finally
        {

            if (m_isBZip2 != null)
            {

                m_isBZip2.Dispose();

            }

            if (m_msBZip2 != null)
            {

                m_msBZip2.Dispose();

            }

        }

        return bytesUncompressed;

    } 

}
	


