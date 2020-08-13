# yae - yet another editor

Console-based text editor tool for .NET Core CLI.

Nuget packages:
- Yae.Core - Console-based text editor<br>
[![NuGet Version](http://img.shields.io/nuget/v/Yae.Core.svg?style=flat)](https://www.nuget.org/packages/Yae.Core/)

- Yae.Tool - Console-based text editor tool for .NET Core CLI<br>
[![NuGet Version](http://img.shields.io/nuget/v/Yae.Tool.svg?style=flat)](https://www.nuget.org/packages/Yae.Tool/)

- Yae.Templates - Templates to use when creating *.cs files<br>
[![NuGet Version](http://img.shields.io/nuget/v/Yae.Templates.svg?style=flat)](https://www.nuget.org/packages/Yae.Templates/)

Yae uses [.NET Core CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/) toolchain for installing.

#### Install

```text
 dotnet tool install -g Yae.Tool
 dotnet new -i Yae.Templates
 ```

#### Uninstall

```text
dotnet tool uninstall -g Yae.Tool
dotnet new -u Yae.Templates
```

### Run tool

Options:

- get help :  *-h | --help*
- get version : *-v | --version*
- open file : *-f | --file \<FILE\>*
- set editor lines per page count : *-n | --count \<COUNT\>*

Examples for global tool:
 
```text
yae -h
yae -v
yae -f file.txt
yae -f C:\file.txt
yae -n 30 -f C:\file.txt
```

Editor window:

![editor](docs/images/yae.png)

### Use templates

There are 4 file templates:
- Class
- Enum
- Interface
- Struct

All of them create files with a base type content in the current directory.

Options:
- set namespace (default current directory) :  *-n \<NAMESPACE\>*
- set output (default current directory) : *-o \<OUTPUT\>* 
- set type name : *-t \<TYPE_NAME\>*

```text
dotnet new class -t Car
dotnet new enum -t Color
dotnet new interface -t IPerson
dotnet new struct -t Point
```