namespace Application.Common.Extensions;

public static class AttributeExtensions
{
    public static TValue GetAttributeValue<TAttribute, TValue>(
        this Type type,
        Func<TAttribute, TValue> valueSelector)
        where TAttribute : Attribute
    {
        var att = type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
        if (att != null)
        {
            return valueSelector(att);
        }

        return default(TValue);
    }
    
    public static TValue GetAttribute<TAttribute, TValue>(this Type type, string memberName, Func<TAttribute, TValue> valueSelector, bool inherit = false) where TAttribute : Attribute
    {
        var att = type.GetMember(memberName).FirstOrDefault().GetCustomAttributes(typeof(TAttribute), inherit).FirstOrDefault() as TAttribute;
        if (att != null)
        {
            return valueSelector(att);
        }
        return default(TValue);
    }
}