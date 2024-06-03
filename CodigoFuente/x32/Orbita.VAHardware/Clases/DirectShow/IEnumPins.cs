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
	// IEnumPins interface
	//
	// Enumerates pins on a filter
	//
	[ComImport,
	Guid("56A86892-0AD4-11CE-B03A-0020AF0BA770"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumPins
	{
		// Retrieves a specified number of pins
		[PreserveSig]
		int Next(
			[In] int cPins,
			[Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] IPin[] ppPins,
			[Out] out int pcFetched);

		// Skips over a specified number of pins
		[PreserveSig]
		int Skip(
			[In] int cPins);

		// Resets the enumeration sequence to the beginning
		[PreserveSig]
		int Reset();

		// Makes a copy of the enumerator with the same enumeration state
		[PreserveSig]
		void Clone(
			[Out] out IEnumPins ppEnum);
	}

}