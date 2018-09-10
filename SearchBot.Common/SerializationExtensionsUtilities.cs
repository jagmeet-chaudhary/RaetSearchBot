using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SearchBot.Common
{
    public static class SerializationExtensionsUtilities
    {
        public static string SerializeObjectToBase64(this object o)
        {
            if (o == null)
            {
                throw new ArgumentNullException(nameof(o));
            }

            if (!o.GetType().IsSerializable)
            {
                throw new InvalidOperationException("SerializationExtensionsUtilities.SerializeObjectToBase64. Error: object received non serializable.");
            }

            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, o);
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        public static object DeserializeObjectFromBase64(this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }
            else if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentOutOfRangeException(str);
            }

            try
            {
                var bytes = Convert.FromBase64String(str);

                using (var stream = new MemoryStream(bytes))
                {
                    return new BinaryFormatter().Deserialize(stream);
                }
            }
            catch (FormatException)
            {
                throw new InvalidOperationException("SerializationExtensionsUtilities.SerializeObjectToBase64. Error: string received non deserializable from base64.");
            }
        }
    }
}
