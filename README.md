# yae - yet another editor

### Summary

Console-based text editor tool for .NET Core CLI.

### Build
 
To setup tool for all directories first run:
```text
cd \
dotnet new tool-manifest
```

To setup tool for current directory just run:
```text
dotnet new tool-manifest
```

To install tool:
 
```text
git clone https://github.com/daredever/yae.git
cd src/YaeTool/
dotnet pack
dotnet tool install --add-source ./nupkg YaeTool
```

To uninstall tool:
 
```text
dotnet tool uninstall YaeTool
```

### Run

Options:

- get help :  *-h | --help*
- get version : *-v | --version*
- open file : *-f | --file Path/To/File*
- set editor lines per page count : *-n | --count LinesPerPageCount*

Examples:
 
```text
dotnet yae -h
dotnet yae -v
dotnet yae -f file.txt
dotnet yae -f C:\file.txt
dotnet yae -n 30 -f C:\file.txt
```

Editor window:

![editor](docs/images/yae.png)
