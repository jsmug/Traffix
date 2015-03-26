using System;

namespace Traffix
{

    public class TypeRegisteredEventArgs : EventArgs
    {

        private object baseInstance;
        private Type baseType;


        private TypeRegisteredEventArgs()
            : base()
        {
        }

        public TypeRegisteredEventArgs(Type type, object instance)
        {

            this.baseInstance = instance;
            this.baseType = type;

        }


        #region public

        public object Instance
        {

            get
            {
                return this.baseInstance;
            }

        }

        public Type Type
        {

            get
            {
                return this.baseType;
            }

        #endregion

        }

    }

}
