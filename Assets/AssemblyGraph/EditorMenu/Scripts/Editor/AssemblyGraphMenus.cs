using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

using Assemblies = System.Collections.Generic.IEnumerable<UnityEditor.Compilation.Assembly>;
using Lines = System.Collections.Generic.IEnumerable<string>;

namespace Plugins.Editor.AssemblyGraph
{
    public static class AssemblyGraphMenus
    {
        const string MenuPath = "Tools/Assembly Graph/";
        const string Player = "Player Assemblies/";
        const string Editor = "Editor Assemblies/";
        const string All = "All Assemblies/";
        const string ExcludeUnity = "Exclude Unity/";
        const string IncludeUnity = "Include Unity/";
        const string GraphViz = "Create GraphViz File (.dot)";
        const string Quickchart = "Show in quickchart.io";

        #region Player_Assemblies

        [MenuItem(MenuPath + Player + ExcludeUnity + GraphViz)]
        public static void CreatePlayerAssemblyGraphInAssetFolder()
        {
            Execute(CreateDotFileInAssetFolder, AssembliesType.Player, ExcludesAssembliesFromUnity);
        }

        [MenuItem(MenuPath + Player + ExcludeUnity + Quickchart)]
        public static void ShowPlayerAssemblyGraphInQuickchart()
        {
            Execute(OpenResultInQuickchart, AssembliesType.Player, ExcludesAssembliesFromUnity);
        }

        [MenuItem(MenuPath + Player + IncludeUnity + GraphViz)]
        public static void CreateFullPlayerAssemblyGraphInAssetFolder()
        {
            Execute(CreateDotFileInAssetFolder, AssembliesType.Player, null);
        }

        [MenuItem(MenuPath + Player + IncludeUnity + Quickchart)]
        public static void ShowFullPlayerAssemblyGraphInQuickchart()
        {
            Execute(OpenResultInQuickchart, AssembliesType.Player, null);
        }

        #endregion Player_Assemblies

        #region Editor_Assemblies

        [MenuItem(MenuPath + Editor + ExcludeUnity + GraphViz)]
        public static void CreateEditorAssemblyGraphInAssetFolder()
        {
            Execute(CreateDotFileInAssetFolder, AssembliesType.Editor, ExcludesAssembliesFromUnity);
        }

        [MenuItem(MenuPath + Editor + ExcludeUnity + Quickchart)]
        public static void ShowEditorAssemblyGraphInQuickchart()
        {
            Execute(OpenResultInQuickchart, AssembliesType.Editor, ExcludesAssembliesFromUnity);
        }

        [MenuItem(MenuPath + Editor + IncludeUnity + GraphViz)]
        public static void CreateFullEditorAssemblyGraphInAssetFolder()
        {
            Execute(CreateDotFileInAssetFolder, AssembliesType.Editor, null);
        }

        [MenuItem(MenuPath + Editor + IncludeUnity + Quickchart)]
        public static void ShowFullEditorAssemblyGraphInQuickchart()
        {
            Execute(OpenResultInQuickchart, AssembliesType.Editor, null);
        }

        #endregion Editor_Assemblies

        #region All_Assemblies

        [MenuItem(MenuPath + All + ExcludeUnity + GraphViz)]
        public static void CreateAssemblyGraphInAssetFolder()
        {
            Execute(CreateDotFileInAssetFolder, ExcludesAssembliesFromUnity);
        }

        [MenuItem(MenuPath + All + ExcludeUnity + Quickchart)]
        public static void ShowAssemblyGraphInQuickchart()
        {
            Execute(OpenResultInQuickchart, ExcludesAssembliesFromUnity);
        }

        [MenuItem(MenuPath + All + IncludeUnity + GraphViz)]
        public static void CreateFullAssemblyGraphInAssetFolder()
        {
            Execute(CreateDotFileInAssetFolder);
        }

        [MenuItem(MenuPath + All + IncludeUnity + Quickchart)]
        public static void ShowFullAssemblyGraphInQuickchart()
        {
            Execute(OpenResultInQuickchart);
        }

        #endregion All_Assemblies

        private static void Execute(Action<Lines> Command, Func<Assemblies, Assemblies> Filter = null)
        {
            var assemblies = CompilationPipeline.GetAssemblies();
            var lines = CreateGraphDescription(assemblies, Filter);
            Command.Invoke(lines);
        }

        private static void Execute(Action<Lines> Command, AssembliesType assembliesType, Func<Assemblies, Assemblies> Filter = null)
        {
            var assemblies = CompilationPipeline.GetAssemblies(assembliesType);
            var lines = CreateGraphDescription(assemblies, Filter);
            Command.Invoke(lines);
        }

        private static Lines CreateGraphDescription(Assemblies assemblies, Func<Assemblies, Assemblies> Filter = null)
        {
            var collection = Filter?.Invoke(assemblies) ?? assemblies;

            var lines = new List<string>();
            lines.Add("digraph {");

            // Nodes
            foreach (var assembly in collection)
            {
                var isEditorAssembly = assembly.flags.HasFlag(AssemblyFlags.EditorAssembly);
                var attributes = isEditorAssembly ? "[style=filled, fillcolor=gray]" : string.Empty;
                lines.Add($"  \"{assembly.name}\"{attributes};");
            }

            lines.Add(string.Empty);

            // Edges
            foreach (var assembly in collection)
            {
                var assemblyReferences = assembly.assemblyReferences;
                var references = Filter?.Invoke(assemblyReferences) ?? assemblyReferences;
                foreach (var reference in references)
                {
                    var line = $"  \"{assembly.name}\"->\"{reference.name}\";";
                    lines.Add(line);
                }
            }
            lines.Add("}");
            return lines;
        }

        private static Assemblies ExcludesAssembliesFromUnity(Assemblies assemblies)
        {
            return assemblies.Where(assembly => !assembly.name.StartsWith("Unity"));
        }

        private static void CreateDotFileInAssetFolder(Lines lines)
        {
            var file = Path.Combine(Application.dataPath, "assembly_info.dot");
            File.WriteAllLines(file, lines);
        }

        private static void OpenResultInQuickchart(Lines lines)
        {
            var serviceURL = "https://quickchart.io/graphviz";
            var urlQuery = string.Join(string.Empty, lines).Replace(" ", string.Empty);
            var url = $"{serviceURL}?graph={urlQuery}";
            Application.OpenURL(url);
        }
    }
}