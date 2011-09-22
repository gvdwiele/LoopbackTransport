using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace LoopbackTransport.Tests
{
    internal static class DocLoader
    {
        /// <summary>
        /// Loads a document instance from a resource
        /// </summary>
        /// <param name="name">Name of the resource</param>
        /// <returns></returns>
        public static Stream GetResourceStream(string name)
        {
            string resource = String.Concat(typeof(DocLoader).Namespace, ".", name);
            Stream uStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource);

            MemoryStream outStream = new MemoryStream();

            BinaryWriter bw = new BinaryWriter(outStream);
            BinaryReader br = new BinaryReader(uStream);

            for (int i = 0; i < uStream.Length; i++)
            {
                bw.Write(br.ReadByte());
            }

            br.Close();
            bw.Flush();
            outStream.Seek(0, SeekOrigin.Begin);
            return outStream;
        }

        public static string CreateTempMachineConfig()
        {
            string filename = Path.GetTempFileName();
            Stream input = DocLoader.GetResourceStream("EmbeddedResources.Machine.config");
            using (Stream file = File.OpenWrite(filename))
            {
                CopyStream(input, file);
            }
            return filename;
        }

        /// <summary>
        /// Copies the contents of input to output. Doesn't close either streams.
        /// </summary>
        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }

    }

}

