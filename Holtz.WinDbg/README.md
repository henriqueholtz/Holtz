# Holtz.WinDbg

### Load modules

- `.loadby sos coreclr`
- `.loadby sos clr`
- `.load %userprofile%\.dotnet\sos\sos.dll`

### Symbols

Note: Rebuild the app will cause a mismatch symbols.

- `.sympath to srv*c:\symcache*https://msdl.microsoft.com/download/symbols`: Load the symbols from the Microsoft Symbol Server
- `!sym noisy`: Show error details when loading symbols
- `* <AppName>!*main*`
- `.symfix `

## Commands

### Heap

- `!heap -s` (list native heaps)
- `!heap -stat -h <heapId>`
- `!heap -flt s <size>`
- `!heap -p -a <HEAP_ENTRY>`

### Dump

- `!dumpdomain`
- `!dumpmodule -mt <ModuleId>`
- `!dumpmodule /d <module>`
- `!DumpHeap -stat` (bottom are worst)
- `!DumpHeap -type System.String -min 500000` (just string greater than X bytes)
- `!DumpObj /d <address>` (show the content)
- `!DumpArray /d <address>`
- `!DumpAssembly /d <assembly>`
- `!dumpstack` (list combined stack)

### GCRoot

- `!gcroot <address>`
- `!gcroot -all`

### Analyze

- `!analyze -v`

### Others

- `!finalizequeue`
- `!eeheap` (list .NET heaps)
- `k` (list native heaps)
- `!clrstack` (list .NET stack)
- `!dso` (display stack objects)
- `!ip2md <address?>` (list managed method at address)
- `.chain` (list loaded plugins)
