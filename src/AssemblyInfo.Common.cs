//------------------------------------------------------------------------------
//     Copyright 2007 The Plethora Project.
//     All rights reserved.
//
//     Refer to the Licence.txt distributed with this file for licencing terms.
//------------------------------------------------------------------------------

using System.Reflection;
using System.Runtime.InteropServices;

// Assembly Properties
#if DEBUG 
[assembly: AssemblyConfiguration("DEBUG")]
#else
[assembly: AssemblyConfiguration("RELEASE")]
#endif
[assembly: AssemblyProduct("The Plethora .NET Project")]
[assembly: AssemblyCopyright("Copyright © The Plethora Project. All rights reserved.")]

// Version
[assembly: AssemblyVersion("0.20.0")]
[assembly: AssemblyFileVersion("0.20.0")]

// Interoperability
[assembly: ComVisible(false)]
