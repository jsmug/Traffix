using Microsoft.Phone.Reactive;
using System;

namespace Traffix.Mapping.Entities
{

    public abstract class EntityProvider
    {

        private readonly Subject<EntityEvent> _entityEventSubject = new Subject<EntityEvent>();
        private IdentificationToken _identifier;
        private string _name;


        #region enums

        public enum EntityUpdateType
        {
            Removed = 1,
            Updated = 0
        }

        #endregion


        private EntityProvider()
        {
        }

        public EntityProvider(string name)
        {

            if(string.IsNullOrEmpty("name"))
            {
                throw new ArgumentNullException("name");
            }

            this.CanPause = true;
            this._identifier = new IdentificationToken();
            this._name = name;

        }


        #region public

        public bool CanPause { get; set; }

        public string Description { get; set; }

        public IObservable<EntityEvent> EntityEvents
        {

            get
            {
                return this._entityEventSubject;
            }

        }

        public IdentificationToken Identifier
        {

            get
            {
                return this._identifier;
            }

        }

        public abstract bool IsRunning { get; }

        public string Name
        {

            get
            {
                return this._name;
            }

        }


        public abstract void Pause();
        public abstract void Reset();
        public abstract void Start();
        public abstract void Start(params object[] args);
        public abstract void Stop();

        #endregion

        #region protected

        protected void UpdateEntity(Entity entity, EntityUpdateType entityUpdateType)
        {
            UpdateEntity(new EntityEvent(entity, entityUpdateType));
        }

        protected void UpdateEntity(EntityEvent entityEvent)
        {

            if (entityEvent == null)
            {
                throw new ArgumentNullException("entityEvent");
            }

            this._entityEventSubject.OnNext(entityEvent);

        }

        #endregion

    }

}
