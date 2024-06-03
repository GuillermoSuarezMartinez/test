//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aibañez
// Created          : 15-11-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Runtime.InteropServices;

namespace Orbita.VAHardware
{
	// ICreateDevEnum interface
	//
	// Creates an enumerator for a list of filters and
	// hardware devices in a specified filter category
	//
	[ComImport,
	Guid("29840822-5B84-11D0-BD3B-00A0C911CE86"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ICreateDevEnum
	{
		// Creates a class enumerator for the specified category
		[PreserveSig]
		int CreateClassEnumerator(
			[In] ref Guid pType,
			[Out] out UCOMIEnumMoniker ppEnumMoniker,
			[In] int dwFlags);
	}
}
