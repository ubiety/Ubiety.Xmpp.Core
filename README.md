# Ubiety XMPP Core

![Logo of the project](https://raw.githubusercontent.com/ubiety/Ubiety.Xmpp.Core/develop/ubiety-logo.png)

> Your XMPP choice for .NET Core

| Branch  | Quality                                                                                                                                                                                  | Travis CI                                                                                                                          | Appveyor                                                                                                                                                                           | Coverage                                                                                                                                                                     |
| ------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Master  | [![CodeFactor](https://www.codefactor.io/repository/github/ubiety/ubiety.xmpp.core/badge)](https://www.codefactor.io/repository/github/ubiety/ubiety.xmpp.core)                          | [![Build Status](https://travis-ci.org/ubiety/Ubiety.Xmpp.Core.svg?branch=master)](https://travis-ci.org/ubiety/Ubiety.Xmpp.Core)  | [![Build status](https://ci.appveyor.com/api/projects/status/xtxujov8b3cl616q/branch/master?svg=true)](https://ci.appveyor.com/project/coder2000/ubiety-xmpp-core/branch/master)   | [![Coverage Status](https://coveralls.io/repos/github/ubiety/Ubiety.Xmpp.Core/badge.svg?branch=master)](https://coveralls.io/github/ubiety/Ubiety.Xmpp.Core?branch=master)   |
| Develop | [![CodeFactor](https://www.codefactor.io/repository/github/ubiety/ubiety.xmpp.core/badge/develop)](https://www.codefactor.io/repository/github/ubiety/ubiety.xmpp.core/overview/develop) | [![Build Status](https://travis-ci.org/ubiety/Ubiety.Xmpp.Core.svg?branch=develop)](https://travis-ci.org/ubiety/Ubiety.Xmpp.Core) | [![Build status](https://ci.appveyor.com/api/projects/status/xtxujov8b3cl616q/branch/develop?svg=true)](https://ci.appveyor.com/project/coder2000/ubiety-xmpp-core/branch/develop) | [![Coverage Status](https://coveralls.io/repos/github/ubiety/Ubiety.Xmpp.Core/badge.svg?branch=develop)](https://coveralls.io/github/ubiety/Ubiety.Xmpp.Core?branch=develop) |

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

- Project homepage: <https://xmpp.dieterlunn.ca>
- Repository: <https://github.com/ubiety/Ubiety.Xmpp.Core/>
- Issue tracker: <https://github.com/ubiety/Ubiety.Xmpp.Core/issues>
  - In case of sensitive bugs like security vulnerabilities, please contact
    issues@dieterlunn.ca directly instead of using issue tracker. We value your effort
    to improve the security and privacy of this project!
- Related projects:
  - Ubiety VersionIt: <https://github.com/ubiety/Ubiety.VersionIt/>
  - Ubiety Toolset: <https://github.com/ubiety/Ubiety.Toolset/>

## Sponsors

### Gold Sponsors

[![Gold Sponsors](https://opencollective.com/ubiety/tiers/gold-sponsor.svg?avatarHeight=36)](https://opencollective.com/ubiety/)

### Silver Sponsors

[![Silver Sponsors](https://opencollective.com/ubiety/tiers/silver-sponsor.svg?avatarHeight=36)](https://opencollective.com/ubiety/)

### Bronze Sponsors

[![Bronze Sponsors](https://opencollective.com/ubiety/tiers/bronze-sponsor.svg?avatarHeight=36)](https://opencollective.com/ubiety/)

## Licensing

The code in this project is licensed under the Apache License version 2.
