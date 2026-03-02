using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class PredefinedAssemblyUtil
{
    enum AssemblyType
    {
        AssemblyCSharp,
        AssemblyCSharpEditor,
        AssemblyCsharpEditorFistPass,
        AssemblyCsharpFirstPass
    }
    
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

    public static List<Type> GetTypes(Type interfaceType)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Dictionary<AssemblyType, Type[]> assemblyTypes = new Dictionary<AssemblyType, Type[]>();
        List<Type> types = new List<Type>();
        
        for (int i = 0; i < assemblies.Length; i++)
        {
            AssemblyType? assemblyType = GetAssemblyType(assemblies[i].GetName().Name);
            if (assemblyType != null)
            {
                assemblyTypes.Add((AssemblyType)assemblyType, assemblies[i].GetTypes());
            }
        }

        if (assemblyTypes.TryGetValue(AssemblyType.AssemblyCSharp, out var mainTypes))
            AddTypesFromAssembly(mainTypes, types, interfaceType);

        if (assemblyTypes.TryGetValue(AssemblyType.AssemblyCsharpFirstPass, out var fpTypes))
            AddTypesFromAssembly(fpTypes, types, interfaceType);

        
        return types;
    }
}