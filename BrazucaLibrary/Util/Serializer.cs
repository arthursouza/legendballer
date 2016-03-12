using System;
using System.IO;
using System.Xml.Serialization;

namespace BrazucaLibrary.Util
{
    /// <summary>
    /// Classe estática para serialização de objetos do jogo
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Serializer<T> where T : new ()
    {
        /// <summary>
        /// Serializes an objeto in an xml file
        /// </summary>
        /// <param name="obj">The object to be serialized</param>
        /// <param name="path">The path to the destination file</param>
        public static void SerializeObject(T obj, string path)
        {
            try
            {
                FileInfo fInfo = new FileInfo(path);

                if (!fInfo.Directory.Exists)
                    fInfo.Directory.Create();

                XmlSerializer serializer = new XmlSerializer(typeof(T));                
                TextWriter textWriter = new StreamWriter(path);
                serializer.Serialize(textWriter, obj);
                textWriter.Close();
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// Deserializes an object
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T DeserializeObject(string path)
        {              
            if (File.Exists(path))
            {
                T obj;  
                XmlSerializer deserializer = new XmlSerializer(typeof(T));
                TextReader textReader = new StreamReader(path);
                obj = (T)deserializer.Deserialize(textReader);
                textReader.Close();
                return obj;
            }

            return new T();
        }
    }
}
