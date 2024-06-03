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
	// IFileSourceFilter interface
	//
	// The IFileSourceFilter interface is exposed by source filters
	// to set the file name and media type of the media file that
	// they are to render
	//
	[ComImport,
	Guid("56A868A6-0Ad4-11CE-B03A-0020AF0BA770"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IFileSourceFilter
	{
		// Load a file and assign it the given media type
		[PreserveSig]
		int Load(
			[In, MarshalAs(UnmanagedType.LPWStr)] string pszFileName,
			[In, MarshalAs(UnmanagedType.LPStruct)] AMMediaType	pmt);

		// Get the currently loaded file name
		[PreserveSig]
		int GetCurFile(
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string pszFileName,
			[Out, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt);
	}
}
