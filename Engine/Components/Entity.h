#pragma once

#include "ComponentsCommon.h"

namespace arch
{
#define INIT_INFO(component) \
	namespace component        \
	{                          \
		struct init_info;        \
	}

	INIT_INFO(transform);

#undef INIT_INFO

	namespace game_object
	{
		struct entity_info
		{
			transform::init_info *transform{nullptr};
		};

		/// <summary> Creates a new game entity. </summary>
		entity create_game_object(const entity_info& info);

		/// <summary> Destroys a game entity. </summary>
		void destroy_game_object(entity e);

		/// <summary> Returns if the entity is alive. </summary>
		bool is_alive(entity e);
	}
}