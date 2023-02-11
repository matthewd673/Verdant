# Verdant

Verdant is a flexible 2D game framework built on MonoGame,
designed to simplify the tedious parts of game development without restricting developer freedom.

Verdant is in early development. APIs may be incomplete, and are subject to (potentially drastic) change without warning.

## Build Info

Currently built on .NET 7.0 for compatibility with MonoGame 3.8.x.

**Note:** MonoGame templates usually generate projects targeting either .NET Core 3.0 or .NET 6.0. Within your game's `.csproj` file, set `TargetFramework` to `net7.0`.

**For easiest use**
1. Download the project files from GitHub
2. Copy the `Verdant` folder to a convenient location
3. Create a new solution for your game, and [a new MonoGame project](https://docs.monogame.net/articles/getting_started/0_getting_started.html) within it
4. Add `Verdant.csproj` to your project solution
5. In your game project, add a reference to the Verdant project

## Demos

- **[LoggingDemo](https://github.com/matthewd673/Verdant/tree/master/Demos/LoggingDemo):** A simple game that makes use of `Debugging.Log` and the LogConsole.
- **[ParticleToy](https://github.com/matthewd673/Verdant/tree/master/Demos/ParticleToy):** Tool to visualize `ParticleSystem` configurations. Entirely unfinished.
- **[PhysicsDemo](https://github.com/matthewd673/Verdant/tree/master/Demos/PhysicsDemo):** Demonstrates all physics bodies.
- **[TopdownShooter](https://github.com/matthewd673/Verdant/tree/master/Demos/TopdownShooter):** A bare-bones topdown shooter demonstrating the `PhysicsEntity`, `Pathfinder`, and `Timer` APIs.

## [API Docs](https://github.com/matthewd673/Verdant/wiki)

Documentation for each class is available on [the wiki](https://github.com/matthewd673/Verdant/wiki). It is automatically generated from documentation comments within the codebase.


## Additional Tools
### MapTools

MapTools is an included tool for creating simple map files built from images, which Verdant can parse. MapTools and its map files can also be used entirely independently of Verdant. More info in the
[MapTools README](https://github.com/matthewd673/Verdant/blob/master/MapTools/README.md).

### LogConsole

LogConsole is a super simple client for the `Debugging.Log` API included in Verdant.
More info is available in the [LogConsole README](https://github.com/matthewd673/Verdant/blob/master/LogConsole/README.md).