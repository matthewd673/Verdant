using System;
using System.Runtime.Serialization;
using System.Reflection;

namespace Verdant.Networking
{
    internal class NetworkEntitySerializationSurrogate : ISerializationSurrogate
    {

        //adapted from:
        //https://stackoverflow.com/questions/61148351/ignore-a-property-during-serialization-using-binaryformatter-only-if-its-null
        void ISerializationSurrogate.GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            if (obj == null)
                return;

            foreach (FieldInfo field in obj.GetType().GetFields(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                ))
            {
                object fieldValue = field.GetValue(obj);
                if (fieldValue == null)
                    continue;

                Type fieldType = fieldValue.GetType();
                if (IsSerializeType(fieldType))
                    info.AddValue(field.Name, fieldValue);
            }

            foreach (PropertyInfo property in obj.GetType().GetProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                ))
            {
                object propertyValue = property.GetValue(obj);
                if (propertyValue == null)
                    continue;

                Type propertyType = propertyValue.GetType();
                if (IsSerializeType(propertyType))
                    info.AddValue(property.Name, propertyValue);
            }
        }

        bool IsSerializeType(Type type)
        {
            //this is certainly not comprehensive, but it'll grow as needed over time
            //
            //a good property for serialization is small (we wouldn't serialize an EntityManager)
            //and commonly used (like Vec2, bool, float, etc.) and likely to be unqiue for each Entity
            //(for example, all Entities of a given type likely have the same Sprite, so don't bother)
            //
            //anything missing can be done custom (and support for custom surrogates should be added some day)

            return (
                type == typeof(int) ||
                type == typeof(string) ||
                type == typeof(bool) ||
                type == typeof(float) ||
                type == typeof(Vec2) ||
                type == typeof(Vec2Int)
                );
        }

        object ISerializationSurrogate.SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            throw new NotImplementedException();
        }

    }
}
