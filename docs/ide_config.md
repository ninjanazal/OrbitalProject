# Setting Up Debugging for Godot Mono in VSCode

This guide explains how to configure Visual Studio Code (VSCode) to debug your Godot Mono projects. By setting up the configurations below, you can launch the Godot executable directly from VSCode, run pre-launch build tasks, and attach the debugger to your game.

> **Note:**  
> - Make sure you have the [Mono version of Godot](https://godotengine.org/download) installed.
> - Ensure you have the .NET SDK installed.
> - Install the C# extension in VSCode.

---

## 1. `launch.json` Configuration

The `launch.json` file defines multiple debugging configurations:

- **Launch**: Runs the game using the workspace path.
- **Launch (Select Scene)**: Prompts you to select a scene to launch.
- **Launch Editor**: Opens the Godot Editor with the current project.
- **Attach to Process**: Attaches the debugger to a running process.

Place the following content in your `.vscode/launch.json` file:

```json
{
	"version": "0.2.0",
	"configurations": [
		{
			"name": "Launch",
			"type": "coreclr",
			"request": "launch",
			"preLaunchTask": "build-debug",
			"program": "Godot_4.x_mono.exe",
			"cwd": "${workspaceFolder}",
			"console": "internalConsole",
			"stopAtEntry": false,
			"args": [
				"--path",
				"${workspaceRoot}"
			]
		},
		{
			"name": "Launch (Select Scene)",
			"type": "coreclr",
			"request": "launch",
			"preLaunchTask": "build",
			"program": "Godot_4.x_mono.exe",
			"cwd": "${workspaceFolder}",
			"console": "internalConsole",
			"stopAtEntry": false,
			"args": [
				"--path",
				"${workspaceRoot}",
				"${command:godot.csharp.getLaunchScene}"
			]
		},
		{
			"name": "Launch Editor",
			"type": "coreclr",
			"request": "launch",
			"preLaunchTask": "build",
			"program": "Godot_4.x_mono.exe",
			"cwd": "${workspaceFolder}",
			"console": "internalConsole",
			"stopAtEntry": false,
			"args": [
				"--path",
				"${workspaceRoot}",
				"--editor"
			]
		},
		{
			"name": "Attach to Process",
			"type": "coreclr",
			"request": "attach"
		}
	]
}
```


**Explanation**
- Launch: Runs the game by using the current workspace as the project path. It executes the pre-launch task "build-debug" to compile your C# scripts in Debug mode.

- Launch (Select Scene): Similar to Launch, but it prompts you to select a scene using the command ${command:godot.csharp.getLaunchScene}. This is useful if you want to debug a specific scene.

- Launch Editor: Opens the Godot Editor with your project. The --editor flag tells Godot to start in editor mode.

- Attach to Process: Allows you to attach the VSCode debugger to an already running Godot process.

--- 

## 2. `tasks.json` Configuration

The `tasks.json` file defines tasks that run before launching the debugger.
In this example, it contains a task called build-debug which compiles your C# scripts using the .NET CLI.

Place the following content in your .vscode/tasks.json file:

```json
{
	"version": "2.0.0",
	"tasks": [
		{
			"label": "build-debug",
			"command": "dotnet",
			"type": "shell",
			"args": [
				"build",
				"/property:GenerateFullPaths=true",
				"-c",
				"Debug",
				"-v",
				"normal"
			],
			"group": {
				"kind": "build",
				"isDefault": true
			},
			"presentation": {
				"reveal": "always"
			},
			"problemMatcher": "$msCompile"
		}
	]
}
```

**Task Details** 
- build-debug: Executes the command dotnet build with parameters to generate full paths, use the Debug configuration (-c Debug), and output verbose logging (-v normal). This task is automatically run before launching any configuration that references it.
