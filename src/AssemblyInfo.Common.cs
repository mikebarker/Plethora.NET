//------------------------------------------------------------------------------
//     Copyright 2007 The Plethora Project.
//     All rights reserved.
//
//     Refer to the Licence.txt distributed with this file for licencing terms.
//------------------------------------------------------------------------------

using System;
using System.Reflection;
using System.Runtime.InteropServices;

// Assembly Properties
#if DEBUG 
[assembly: AssemblyConfiguration("DEBUG")]
#else
[assembly: AssemblyConfiguration("RELEASE")]
#endif
[assembly: AssemblyCompany("Optium ltd")]
[assembly: AssemblyProduct("The Plethora .NET Project")]
[assembly: AssemblyCopyright("Copyright ©  2011")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Version
[assembly: AssemblyVersion("0.16.*")]
[assembly: AssemblyFileVersion("0.16.0.1")]

// Interoperability
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
