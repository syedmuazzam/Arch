#pragma once

#define USE_STL_VECTOR 1
#define USE_STIL_DEQUE 1

#if USE_STL_VECTOR
#include <vector>
namespace arch::utl
{
	template <typename T>
	using vector = std::vector<T>;
}
#endif

#if USE_STIL_DEQUE
#include <deque>
namespace arch::utl
{
	template <typename T>
	using deque = std::deque<T>;
}
#endif

namespace arch::utl
{
	// TODO: implement our own containers
}