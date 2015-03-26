using System;

namespace Traffix.Mapping.Entities
{

    public class EntityEvent
    {

        private Entity _entity;
        private EntityProvider.EntityUpdateType _entityUpdateType;


        public EntityEvent()
        {
        }

        public EntityEvent(Entity entity, EntityProvider.EntityUpdateType entityUpdateType)
        {

            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (entityUpdateType < EntityProvider.EntityUpdateType.Updated || entityUpdateType > EntityProvider.EntityUpdateType.Removed)
            {
                throw new ArgumentOutOfRangeException("entityUpdateType");
            }

            this._entity = entity;
            this._entityUpdateType = entityUpdateType;

        }

        #region public

        public Entity Entity
        {

            get
            {
                return this._entity;
            }

            set
            {

                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this._entity = value;

            }

        }

        public EntityProvider.EntityUpdateType EntityUpdateType
        {

            get
            {
                return this._entityUpdateType;
            }

            set
            {

                if (value < EntityProvider.EntityUpdateType.Updated || value > EntityProvider.EntityUpdateType.Removed)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                this._entityUpdateType = value;

            }

        }

        #endregion

    }

}
