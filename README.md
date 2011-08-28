# Extemory
Extemory is a C# memory editing and hacking library capable of performing a variety of useful functions. The majority of the interface is exposed as extension methods to primitive types.

Currently Extemory is capable of performing the following tasks:

* Read/write local process native memory
* Read/write other processes' native memory
* Inject native DLL into target process
  * Execute exported functions on injected DLL
* Detour native functions with .NET delegates in local process

## License
Extemory is licensed under the FreeBSD or Simplified BSD License.

## Examples
### Local Read/Write

Read a primitive type from an unmanaged memory location

    var pAddr = new IntPtr(0xDEADBEEF);
    var result = pAddr.Read<int>();

Or read from the address explicitly

    var result = 0xDEADBEEF.Read<double>();

Read an address and write an array of primitive types to the address

    var vals = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
    var pAddr = 0xDEADBEEF.Read<int>();
    pAddr.WriteArray<byte>(vals);

### Process Read/Write

The API is the same as for local, except with the extensions on the System.Diagnostics.Process object.

    var proc = Process.GetProcessesByName("notepad").First();
    var someProcConstant = proc.Read<int>(0xDEADBEEF);


### DLL Injection

Inject a native DLL into 'notepad' and then execute 'MyExport' function

    var proc = Process.GetProcessesByName("notepad").First();
    var injectedModule = proc.InjectLibrary(@"c:\Path\To\My\Native.dll"); // or proc.InjectLibrary("Local.dll");
    var result = injectedModule.CallExport("MyExport");