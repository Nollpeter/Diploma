namespace BlazorCraft.Web.Infrastructure;

public static class TypeExtensions
{
    public static string TypeNameWithoutGenerics(this Type componentType) 
        => componentType.IsGenericType ? componentType.Name.Split('`')[0] : componentType.Name;
}