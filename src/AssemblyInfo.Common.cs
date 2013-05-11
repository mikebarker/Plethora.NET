﻿//------------------------------------------------------------------------------
//     Copyright 2007 The Plethora Project.
//     All rights reserved.
//
//     Refer to the Licence.txt distributed with this file for licencing terms.
//------------------------------------------------------------------------------

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

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
[assembly: AssemblyVersion("0.10.*")]
[assembly: AssemblyFileVersion("0.10.0.1")]

// Interoperability
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
