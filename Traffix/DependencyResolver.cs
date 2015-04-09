using System;
using System.Collections.Generic;

namespace Traffix
{
    
    public static class DependencyResolver
    {

        private static readonly object _lock = new object();
        private static readonly IDictionary<Type, object> _registeredObjects = new Dictionary<Type, object>();


        public static event EventHandler<TypeRegisteredEventArgs> TypeRegistered;


        #region public

        public static void Register(Type type, object instance)
        {

            lock (_lock)
            {

                if (!_registeredObjects.ContainsKey(type))
                {

                    if (instance != null && !type.IsAssignableFrom(instance.GetType()))
                    {
                        throw new InvalidOperationException(string.Format(Resources.InvalidTypeRegistration, type.ToString()));
                    }

                    _registeredObjects.Add(type, instance);

                    if (TypeRegistered != null)
                    {
                        TypeRegistered(null, new TypeRegisteredEventArgs(type, instance));
                    }

                }

            }

        }

        public static object Resolve(Type type)
        {

            lock (_lock)
            {

                object instance = null;

                if (_registeredObjects.ContainsKey(type))
                {

                    instance = _registeredObjects[type];

                    if (instance == null)
                    {
                        instance = Activator.CreateInstance(type);
                    }

                }

                return instance;

            }

        }

        public static T Resolve<T>()
        {

            object instance = Resolve(typeof(T));

            lock (_lock)
            {

                T resolved = default(T);

                if (instance != null)
                {

                    try
                    {
                        resolved = (T)instance;
                    }
                    catch
                    {
                    }

                }

                return resolved;

            }

        }

        public static void Remove(Type type)
        {

            lock(_lock)
            {

                if (_registeredObjects.ContainsKey(type))
                {
                    _registeredObjects.Remove(type);
                }

            }

        }

        #endregion

    }

}
