using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 预定义程序集工具类，用于从特定Unity程序集中获取实现指定接口的类型
/// </summary>
public static class PredefinedAssemblyUtil
{
    /// <summary>
    /// 定义Unity中常见的程序集类型枚举
    /// </summary>
    enum AssemblyType
    {
        AssemblyCSharp,
        AssemblyCSharpEditor,
        AssemblyCsharpEditorFistPass,
        AssemblyCsharpFirstPass
    }

    /// <summary>
    /// 根据程序集名称获取对应的程序集类型
    /// </summary>
    /// <param name="assemblyName">程序集名称</param>
    /// <returns>对应的程序集类型，如果未找到则返回null</returns>
    private static AssemblyType? GetAssemblyType(string assemblyName)
    {
        return assemblyName switch
        {
            "Assembly-CSharp" => AssemblyType.AssemblyCSharp,
            "Assembly-CSharp-Editor" => AssemblyType.AssemblyCSharpEditor,
            "Assembly-Csharp-Editor-FirstPass" => AssemblyType.AssemblyCsharpEditorFistPass,
            "Assembly-Csharp-FirstPass" => AssemblyType.AssemblyCsharpFirstPass,
            _ => null
        };
    }

    /// <summary>
    /// 从指定程序集类型数组中筛选出实现指定接口的类型并添加到结果集合中
    /// </summary>
    /// <param name="assembly">程序集中的类型数组</param>
    /// <param name="types">用于存储符合条件类型的集合</param>
    /// <param name="interfaceType">目标接口类型</param>
    private static void AddTypesFromAssembly(Type[] assembly, ICollection<Type> types, Type interfaceType)
    {
        if (assembly == null) return;
        for (int i = 0; i < assembly.Length; i++)
        {
            Type type = assembly[i];
            if (type != interfaceType && interfaceType.IsAssignableFrom(type))
            {
                types.Add(type);
            }
        }
    }

    /// <summary>
    /// 获取所有实现指定接口的类型列表
    /// </summary>
    /// <param name="interfaceType">要查找的接口类型</param>
    /// <returns>实现指定接口的所有类型列表</returns>
    public static List<Type> GetTypes(Type interfaceType)
    {
        // 获取当前应用程序域中的所有程序集
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Dictionary<AssemblyType, Type[]> assemblyTypes = new Dictionary<AssemblyType, Type[]>();
        List<Type> types = new List<Type>();

        // 遍历所有程序集，将预定义的程序集类型及其类型数组存入字典
        for (int i = 0; i < assemblies.Length; i++)
        {
            AssemblyType? assemblyType = GetAssemblyType(assemblies[i].GetName().Name);
            if (assemblyType != null)
            {
                assemblyTypes.Add((AssemblyType)assemblyType, assemblies[i].GetTypes());
            }
        }

        // 按优先级顺序从主程序集和第一遍编译程序集中查找实现指定接口的类型
        if (assemblyTypes.TryGetValue(AssemblyType.AssemblyCSharp, out var mainTypes))
            AddTypesFromAssembly(mainTypes, types, interfaceType);

        if (assemblyTypes.TryGetValue(AssemblyType.AssemblyCsharpFirstPass, out var fpTypes))
            AddTypesFromAssembly(fpTypes, types, interfaceType);


        return types;
    }
}