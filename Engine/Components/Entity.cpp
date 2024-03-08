#include "Entity.h"
#include "Transform.h"

namespace arch::game_object
{
	// anonymous namespace to make the following variables local to this
	namespace
	{
		utl::vector<transform::component> transforms;
		utl::vector<id::generation_type> generations;
		utl::deque<entity_id> free_indices;
	}

	entity create_game_object(const entity_info& info)
	{
		assert(info.transform);
		if (!info.transform)
			return entity{};

		entity_id id;
		if (free_indices.size() > id::min_deleted_indices)
		{
			id = free_indices.front();
			assert(!is_alive(entity{ id }));
			free_indices.pop_front();
			id = entity_id{ id::new_generation(id) };
			++generations[id::index(id)];
		}
		else
		{
			id = entity_id{ id::id_type(generations.size()) };
			generations.push_back(0);

			transforms.emplace_back();
		}

		const entity new_entity{ id };
		const id::id_type index{ id::index(id) };

		// create the transform component
		assert(!transforms[index].is_valid());
		transforms[index] = transform::create_transform(*info.transform, new_entity);
		if (!transforms[index].is_valid())
			return entity{};

		return new_entity;
	}

	void destroy_game_object(entity e)
	{
		const entity_id id{ e.get_id() };
		const id::id_type index{ id::index(id) };
		assert(is_alive(e));
		if (is_alive(e))
		{
			transform::remove_transform(transforms[index]);
			transforms[index] = transform::component{};
			free_indices.push_back(id);
		}
	}

	bool is_alive(entity e)
	{
		assert(e.is_valid());
		const entity_id id{ e.get_id() };
		const id::id_type index{ id::index(id) };
		assert(index < generations.size());
		assert(generations[index] == id::generation(id));
		return generations[index] == id::generation(id) && transforms[index].is_valid();
	}

	transform::component entity::transform() const
	{
		assert(is_alive(*this));
		const id::id_type index{ id::index(_id) };
		return transforms[index];
	}
}