using System;

namespace Traffix
{

    public class TypeRegisteredEventArgs : EventArgs
    {

        private object _instance;
        private Type _type;


        private TypeRegisteredEventArgs()
            : base()
        {
        }

        public TypeRegisteredEventArgs(Type type, object instance)
        {

            this._instance = instance;
            this._type = type;

        }


        #region public

        public object Instance
        {

            get
            {
                return this._instance;
            }

        }

        public Type Type
        {

            get
            {
                return this._type;
            }

        #endregion

        }

    }

}
