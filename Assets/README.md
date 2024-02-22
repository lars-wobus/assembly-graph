# AssemblyGraph

## EditorMenu

1. Import this folder if you want to visualize the dependencies between assemblies.
2. To visualize the dependencies in your project, select Tools > Assembly Graph > (...)

## Demo_ObjectAdapterPattern

This folder is optional and only provided in case you want to see how you can minimize dependencies between assemblies.

1. Start the provided scene and study the console output.
2. Then open MainBehaviour.cs and remove the preprocessor instructions.
3. Save the changes and start the scene again.
4. The application still works, but now uses a different script from a different assembly.

You can now develop your own variants within additional assemblies and simply exchange them. The advantage of implementing additional adapters is that most assemblies do not need to know each other. In other words, you can reduce the number of dependencies within your project if you use this code convention.