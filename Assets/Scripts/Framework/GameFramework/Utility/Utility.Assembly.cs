//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework
{
    public static partial class Utility
    {
        /// <summary>
        /// 程序集相关的实用函数。
        /// </summary>
        public static class Assembly
        {
            private static readonly List<System.Reflection.Assembly> s_Assemblies = new();
            private static readonly Dictionary<string, Type> s_CachedTypes = new(StringComparer.Ordinal);
            public static System.Reflection.Assembly CommonAssembly { get; set; }
			public static System.Reflection.Assembly ClientAssembly { get; set; }
			public static System.Reflection.Assembly ServerAssembly { get; set; }

			static Assembly()
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();

                foreach (var assembly in assemblies)
                {
#if !UNITY_EDITOR
                    if(assembly.FullName.StartsWith("Common,") || assembly.FullName.StartsWith("Client,") || assembly.FullName.StartsWith("Server,"))
                    {
						continue;
                    }
#else
                    if(assembly.FullName.StartsWith("Common,"))
					{
						CommonAssembly = assembly;
					}
                    else if (assembly.FullName.StartsWith("Client,"))
                    {
						ClientAssembly = assembly;
					}
                    else if (assembly.FullName.StartsWith("Server,"))
                    {
						ServerAssembly = assembly;
					}
#endif
					s_Assemblies.Add(assembly);
				}
            }

            /// <summary>
            /// 获取已加载的程序集。
            /// </summary>
            /// <returns>已加载的程序集。</returns>
            public static List<System.Reflection.Assembly> GetAssemblies()
            {
                return s_Assemblies;
            }

            public static void AddAssembliy(System.Reflection.Assembly assembly)
            {
				if (assembly == null)
                {
					throw new GameFrameworkException("Assembly is invalid.");
				}
				if (s_Assemblies.Contains(assembly))
                {
					throw new GameFrameworkException("Assembly is already exist.");
				}
				s_Assemblies.Add(assembly);
			}

			/// <summary>
			/// 获取已加载的程序集中的所有类型。
			/// </summary>
			/// <returns>已加载的程序集中的所有类型。</returns>
			public static Type[] GetTypes()
            {
                List<Type> results = new List<Type>();
                foreach (System.Reflection.Assembly assembly in s_Assemblies)
                {
                    results.AddRange(assembly.GetTypes());
                }

                return results.ToArray();
            }

            /// <summary>
            /// 获取已加载的程序集中的所有类型。
            /// </summary>
            /// <param name="results">已加载的程序集中的所有类型。</param>
            public static void GetTypes(List<Type> results)
            {
                if (results == null)
                {
                    throw new GameFrameworkException("Results is invalid.");
                }

                results.Clear();
                foreach (System.Reflection.Assembly assembly in s_Assemblies)
                {
                    results.AddRange(assembly.GetTypes());
                }
            }

            /// <summary>
            /// 获取已加载的程序集中的指定类型。
            /// </summary>
            /// <param name="typeName">要获取的类型名。</param>
            /// <returns>已加载的程序集中的指定类型。</returns>
            public static Type GetType(string typeName)
            {
                if (string.IsNullOrEmpty(typeName))
                {
                    throw new GameFrameworkException("Type name is invalid.");
                }

				if (s_CachedTypes.TryGetValue(typeName, out Type type))
				{
					return type;
				}

				type = Type.GetType(typeName);
                if (type != null)
                {
                    s_CachedTypes.Add(typeName, type);
                    return type;
                }

                foreach (System.Reflection.Assembly assembly in s_Assemblies)
                {
                    type = assembly.GetType(typeName);
					if (type != null)
                    {
						s_CachedTypes.Add(typeName, type);
                        return type;
                    }
                }

                return null;
            }
        }
    }
}
