# FSharp Tic Tac Toe

This is an example implementation of how to use [Fable](https://fable.io) and [Saturn](https://saturnframework.org/) to create a basic game, tic tac toe. The initial codebase was generated using the [SAFE Framework](https://safe-stack.github.io/) [quickstart](https://safe-stack.github.io/docs/quickstart/).

# Getting started

Before running you'll need to ensure you have [FAKE 5](https://fake.build/) installed, which is easily done using a global `dotnet tool`:

```sh
dotnet tool install fake-cli -g
```

Now you can build and run using FAKE:

```sh
fake build
```

And launch it from FAKE:

```sh
fake build --target run
```

## Recommended tools

For local development you can either use Visual Studio 2017 with update 15.8 installed or VSCode with [Ionide](http://ionide.io/). There are two VSCode tasks defined, `fake:build` and `fake:run` that you can use to build/run it from VSCode.

## Troubleshooting

* Make sure you have .NET Core 2.1 installed
* Make sure you have node.js 10+ with [yarn](https://yarnpkg.com/)
* Make sure that FAKE is at least v5 stable
* To debug the server you're best bet is to attach to the process `dotnet exec <path to Server.dll>`
