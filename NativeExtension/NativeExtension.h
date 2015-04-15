// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the NATIVEEXTENSION_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// NATIVEEXTENSION_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef NATIVEEXTENSION_EXPORTS
#define NATIVEEXTENSION_API __declspec(dllexport)
#else
#define NATIVEEXTENSION_API __declspec(dllimport)
#endif

// This class is exported from the NativeExtension.dll
class NATIVEEXTENSION_API CNativeExtension {
public:
	CNativeExtension(void);
	// TODO: add your methods here.
};

extern NATIVEEXTENSION_API int nNativeExtension;

NATIVEEXTENSION_API int fnNativeExtension(void);
