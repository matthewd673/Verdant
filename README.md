# IsoEngine

A flexible game engine built on MonoGame,
designed to simplify the tedious parts of game development without restricting developer freedom.

**NOTE:** IsoEngine is in early development. APIs may be incomplete, and are subject to (potentially drastic) change without warning.

## Build Info

Currently built on `.NET 5.0` for compatibility with `MonoGame 3.x`.

**For easiest use**
1. Download the project files from GitHub
2. Copy the `IsoEngine/` folder to a convenient location
3. Add `IsoEngine.csproj` to your project solution
4. In your game project, add a reference to the IsoEngine project

## MapTools

MapTools is an included tool for creating simple map files built from images, which IsoEngine can parse. MapTools and its map files can be used entirely independently of IsoEngine. More info in the 
[MapTools README](https://github.com/matthewd673/IsoEngine/blob/master/MapTools/README.md).

## LogConsole

LogConsole is a super simple client for the `Debugging.Log` API included in IsoEngine.
After running LogConsole, begin debugging any project that uses `Debugging.Log` and the messages
will appear LogConsole.

## Demos

- **[LoggingDemo](https://github.com/matthewd673/IsoEngine/tree/master/Demos/LoggingDemo):** A simple game that makes use of `Debugging.Log` and the LogConsole.
- **[ParticleToy](https://github.com/matthewd673/IsoEngine/tree/master/Demos/ParticleToy):** Tool to visualize `ParticleSystem` configurations. Entirely unfinished.
- **[PhysicsDemo](https://github.com/matthewd673/IsoEngine/tree/master/Demos/PhysicsDemo):** Demonstrates all physics bodies to simplify debugging.

## [Documentation](https://github.com/matthewd673/IsoEngine/wiki)

Documentation for each class is available on [the wiki](https://github.com/matthewd673/IsoEngine/wiki). It is automatically generated from documentation comments within the codebase.
