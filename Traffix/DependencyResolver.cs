using System;
using System.Collections.Generic;

namespace Traffix
{
    
    public static class DependencyResolver
    {

        private static readonly object baseLock = new object();
        private static readonly IDictionary<Type, object> baseRegisteredObjects = new Dictionary<Type, object>();


        public static event EventHandler<TypeRegisteredEventArgs> TypeRegistered;


        #region public

        public static void Register(Type type, object instance)
        {

            lock (baseLock)
            {

                if (!baseRegisteredObjects.ContainsKey(type))
                {

                    if (instance != null && !type.IsAssignableFrom(instance.GetType()))
                    {
                        throw new InvalidOperationException(string.Format(Resources.InvalidTypeRegistration, type.ToString()));
                    }

                    baseRegisteredObjects.Add(type, instance);

                    if (TypeRegistered != null)
                    {
                        TypeRegistered(null, new TypeRegisteredEventArgs(type, instance));
                    }

                }

            }

        }

        public static object Resolve(Type type)
        {

            lock (baseLock)
            {

                object instance = null;

                if (baseRegisteredObjects.ContainsKey(type))
                {

                    instance = baseRegisteredObjects[type];

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

            lock (baseLock)
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

            lock(baseLock)
            {

                if (baseRegisteredObjects.ContainsKey(type))
                {
                    baseRegisteredObjects.Remove(type);
                }

            }

        }

        #endregion

    }

}
