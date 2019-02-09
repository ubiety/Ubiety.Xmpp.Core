![Logo of the project](https://raw.githubusercontent.com/jehna/readme-best-practices/master/sample-logo.png)

# Ubiety XMPP Core

> Your XMPP choice for .NET Core

[![CodeFactor](https://www.codefactor.io/repository/github/ubiety/ubiety.xmpp.core/badge)](https://www.codefactor.io/repository/github/ubiety/ubiety.xmpp.core) [![Build Status](https://travis-ci.org/ubiety/Ubiety.Xmpp.Core.svg?branch=master)](https://travis-ci.org/ubiety/Ubiety.Xmpp.Core) [![Build status](https://ci.appveyor.com/api/projects/status/xtxujov8b3cl616q/branch/master?svg=true)](https://ci.appveyor.com/project/coder2000/ubiety-xmpp-core/branch/master)
[![Coverage Status](https://coveralls.io/repos/github/ubiety/Ubiety.Xmpp.Core/badge.svg?branch=master)](https://coveralls.io/github/ubiety/Ubiety.Xmpp.Core?branch=master)

XMPP is not dying and this library will help keep it alive. You can use this to
develop a client for this popular messaging protocol and soon it will support
small server instances.

## Installing / Getting started

Ubiety XMPP Core is available from NuGet

```shell
dotnet package install Ubiety.Xmpp.Core
```

You can also use your favorite NuGet client.

## Developing

Here's a brief intro about what a developer must do in order to start developing
the project further:

```shell
git clone https://github.com/ubiety/Ubiety.Xmpp.Core.git
cd Ubiety.Xmpp.Core
dotnet restore
```

Clone the repository and then restore the development requirements. You can use
any editor, Rider, VS Code or VS 2017. The library supports all .NET Core
platforms.

### Building

Building is simple

```shell
dotnet build
```

### Deploying / Publishing

```shell
git pull
versionize
dotnet pack
dotnet nuget push
git push
```

And again you'd need to tell what the previous code actually does.

## Features

What's all the bells and whistles this project can perform?

- What's the main functionality
- You can also do another thing
- If you get really randy, you can even do this

## Configuration

Here you should write what are all of the configurations a user can enter when
using the project.

#### Argument 1

Type: `String`  
Default: `'default value'`

State what an argument does and how you can use it. If needed, you can provide
an example below.

Example:

```bash
awesome-project "Some other value"  # Prints "You're nailing this readme!"
```

#### Argument 2

Type: `Number|Boolean`  
Default: 100

Copy-paste as many of these as you need.

## Contributing

When you publish something open source, one of the greatest motivations is that
anyone can just jump in and start contributing to your project.

These paragraphs are meant to welcome those kind souls to feel that they are
needed. You should state something like:

"If you'd like to contribute, please fork the repository and use a feature
branch. Pull requests are warmly welcome."

If there's anything else the developer needs to know (e.g. the code style
guide), you should link it here. If there's a lot of things to take into
consideration, it is common to separate this section to its own file called
`CONTRIBUTING.md` (or similar). If so, you should say that it exists here.

## Links

Even though this information can be found inside the project on machine-readable
format like in a .json file, it's good to include a summary of most useful
links to humans using your project. You can include links like:

- Project homepage: https://your.github.com/awesome-project/
- Repository: https://github.com/your/awesome-project/
- Issue tracker: https://github.com/your/awesome-project/issues
  - In case of sensitive bugs like security vulnerabilities, please contact
    my@email.com directly instead of using issue tracker. We value your effort
    to improve the security and privacy of this project!
- Related projects:
  - Your other project: https://github.com/your/other-project/
  - Someone else's project: https://github.com/someones/awesome-project/

## Licensing

One really important part: Give your project a proper license. Here you should
state what the license is and how to find the text version of the license.
Something like:

"The code in this project is licensed under MIT license."
