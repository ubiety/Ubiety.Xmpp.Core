# ![Logo](https://raw.githubusercontent.com/ubiety/Ubiety.Xmpp.Core/develop/images/messages64.png) Ubiety XMPP Core ![Nuget](https://img.shields.io/nuget/v/Ubiety.Xmpp.Core.svg?style=flat-square)

> Your XMPP choice for .NET Core

| Branch  | Quality                                                                                                                                                                                                    | Travis CI                                                                                                                                                     | Appveyor                                                                                                                                                                                     | Coverage                                                                                                                                                                     |
| ------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Master  | [![CodeFactor](https://www.codefactor.io/repository/github/ubiety/ubiety.xmpp.core/badge?style=flat-square)](https://www.codefactor.io/repository/github/ubiety/ubiety.xmpp.core)                          | [![Travis (.org) branch](https://img.shields.io/travis/ubiety/Ubiety.Xmpp.Core/master.svg?style=flat-square)](https://travis-ci.org/ubiety/Ubiety.Xmpp.Core)  | [![AppVeyor branch](https://img.shields.io/appveyor/ci/coder2000/ubiety-xmpp-core/master.svg?style=flat-square)](https://ci.appveyor.com/project/coder2000/ubiety-xmpp-core/branch/master)   | [![Codecov branch](https://img.shields.io/codecov/c/gh/ubiety/Ubiety.Xmpp.Core/master.svg?style=flat-square)](https://codecov.io/gh/ubiety/Ubiety.Xmpp.Core)                 |
| Develop | [![CodeFactor](https://www.codefactor.io/repository/github/ubiety/ubiety.xmpp.core/badge/develop?style=flat-square)](https://www.codefactor.io/repository/github/ubiety/ubiety.xmpp.core/overview/develop) | [![Travis (.org) branch](https://img.shields.io/travis/ubiety/Ubiety.Xmpp.Core/develop.svg?style=flat-square)](https://travis-ci.org/ubiety/Ubiety.Xmpp.Core) | [![AppVeyor branch](https://img.shields.io/appveyor/ci/coder2000/ubiety-xmpp-core/develop.svg?style=flat-square)](https://ci.appveyor.com/project/coder2000/ubiety-xmpp-core/branch/develop) | [![Codecov branch](https://img.shields.io/codecov/c/gh/ubiety/Ubiety.Xmpp.Core/develop.svg?style=flat-square)](https://codecov.io/gh/ubiety/Ubiety.Xmpp.Core/branch/develop) |

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

If you'd like to contribute, please fork the repository and use a feature
branch. Pull requests are warmly welcome.

## Links

-   Project homepage: https://xmpp.ubiety.ca
-   Repository: <https://github.com/ubiety/Ubiety.Xmpp.Core/>
-   Issue tracker: <https://github.com/ubiety/Ubiety.Xmpp.Core/issues>
    -   In case of sensitive bugs like security vulnerabilities, please use the
        [Tidelift security contact](https://tidelift.com/security) instead of using issue tracker.
        We value your effort to improve the security and privacy of this project! Tidelift will coordinate the fix and disclosure.
-   Related projects:
    -   Ubiety VersionIt: <https://github.com/ubiety/Ubiety.VersionIt/>
    -   Ubiety Toolset: <https://github.com/ubiety/Ubiety.Toolset/>
    -   Ubiety Dns: <https://github.com/ubiety/Ubiety.Dns.Core/>
    -   Ubiety Stringprep: <https://github.com/ubiety/Ubiety.Stringprep.Core/>
    -   Ubiety SCRAM: <https://github.com/ubiety/Ubiety.Scram.Core/>

## Sponsors

### Gold Sponsors

[![Gold Sponsors](https://opencollective.com/ubiety/tiers/gold-sponsor.svg?avatarHeight=36)](https://opencollective.com/ubiety/)

### Silver Sponsors

[![Silver Sponsors](https://opencollective.com/ubiety/tiers/silver-sponsor.svg?avatarHeight=36)](https://opencollective.com/ubiety/)

### Bronze Sponsors

[![Bronze Sponsors](https://opencollective.com/ubiety/tiers/bronze-sponsor.svg?avatarHeight=36)](https://opencollective.com/ubiety/)

## Licensing

The code in this project is licensed under the Apache License version 2.
