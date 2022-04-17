# Verdant LogConsole

An easy-to-use debug console for games.

## Usage

Simply build and run the LogConsole project and you'll be ready to recieve log messages.
LogConsole must be running before the game first attempts to write to the log or else no other messages will be sent.
This is due to performance constraints.

The `Debugging.Log` class is used to print messages to LogConsole.
[This wiki article](https://github.com/matthewd673/Verdant/wiki/Debugging.Log) covers the API more thoroughly.
Calls to `WriteLine` will only trigger when Verdant is running under a debugger,
but should still probably be removed from release builds.

**NOTE:** The LogConsole tool and `Debugging.Log` APIs are only intended for local development use on ports that are not public-facing.
Other debugging tools like `Debugging.SimpleStats` that don't use network features can be used however you'd like.