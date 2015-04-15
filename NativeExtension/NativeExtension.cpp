// NativeExtension.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "NativeExtension.h"


// This is an example of an exported variable
NATIVEEXTENSION_API int nNativeExtension=0;

// This is an example of an exported function.
NATIVEEXTENSION_API int fnNativeExtension(void)
{
	return 42;
}

// This is the constructor of a class that has been exported.
// see NativeExtension.h for the class definition
CNativeExtension::CNativeExtension()
{
	return;
}
