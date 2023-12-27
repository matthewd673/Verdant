using System;
using System.Collections.Generic;

using Verdant.Physics;

namespace Verdant;

internal class EntityList
{
    // TODO: in the future, hash this further for lookup by type
    private List<Entity> entities;
    private List<PhysicsEntity> physicsEntities;

    public EntityList()
    {
        entities = new();
        physicsEntities = new();
    }

    public void Add(Entity e)
    {
        if (e.IsType<PhysicsEntity>())
            physicsEntities.Add((PhysicsEntity)e);

        entities.Add(e);
    }

    public bool Remove(Entity e)
    {
        // this is a little weird
        if (e.IsType<PhysicsEntity>())
            physicsEntities.Remove((PhysicsEntity)e);

        return entities.Remove(e);
    }

    public List<Entity> GetEntities()
    {
        return entities;
    }

    public List<PhysicsEntity> GetPhysicsEntities()
    {
        return physicsEntities;
    }
}

