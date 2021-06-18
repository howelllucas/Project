using System;
using System.Reflection;

public class ReflectionUtil
{
    public static U GetValueByProperty<T,U>(string propertyName, T instance)
    {
        PropertyInfo property = typeof(T).GetProperty(propertyName);
        if (property != null)
        {
            return (U)property.GetValue(instance, null);
        } else
        {
            return default(U);
        }
    }

    public static T DeepCopyByReflection<T>(T obj)
    {
        if (obj is string || obj.GetType().IsValueType)
            return obj;

        object retval = Activator.CreateInstance(obj.GetType());
        FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
        foreach (var field in fields)
        {
            try
            {
                field.SetValue(retval, DeepCopyByReflection(field.GetValue(obj)));
            }
            catch { }
        }

        return (T)retval;
    }

}