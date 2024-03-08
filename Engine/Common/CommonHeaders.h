#pragma once

#pragma warning(disable: 4530) // C++ exception handler used, but unwind semantics are not enabled. Specify /EHsc

// C/C++ includes
#include <assert.h>
#include <typeinfo>

#if defined(_WIN64)
#include <DirectXMath.h>
#endif

// Common Headers
#include "PrimitiveTypes.h"
#include "../Utilities/Utilities.h"
#include "../Utilities/MathTypes.h"