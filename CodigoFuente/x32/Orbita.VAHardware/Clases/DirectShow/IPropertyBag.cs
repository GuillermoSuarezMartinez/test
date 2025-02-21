//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aiba�ez
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
	// IPropertyBag
	//
	// The IPropertyBag interface provides an object with a property
	// bag in which the object can persistently save its properties
	//
	[ComImport,
	Guid("55272A00-42CB-11CE-8135-00AA004BB851"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPropertyBag
	{
		// Asks the property bag to read the named property
		[PreserveSig]
		int Read(
			[In, MarshalAs(UnmanagedType.LPWStr)] string pszPropName,
			[In, Out, MarshalAs(UnmanagedType.Struct)] ref object pVar,
			[In] IntPtr pErrorLog);

		// Asks the property bag to save the named property
		[PreserveSig]
		int Write(
			[In, MarshalAs(UnmanagedType.LPWStr)] string pszPropName,
			[In, MarshalAs(UnmanagedType.Struct)] ref object pVar);
	}
}
