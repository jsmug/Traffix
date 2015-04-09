using System;

namespace Traffix.Mapping
{

    public class IdentificationToken
    {

        private Guid _token;


        public IdentificationToken()
        {
            this._token = Guid.NewGuid();
        }


        #region public

        public bool Equals(IdentificationToken other)
        {

            bool equals = false;

            if (other != null)
            {
                equals = Equals(this._token, other.Token);
            }

            return equals;

        }

        public override bool Equals(object obj)
        {
        
            bool equals = false;

            if(ReferenceEquals(this,obj))
            {
               equals = true; 
            }
            else
            {
                equals = Equals(obj as IdentificationToken);
            }

            return equals;

        }

        public override int GetHashCode()
        {
            return this._token.GetHashCode();
        }

        #endregion

        #region internal 

        internal Guid Token
        {

            get
            {
                return this._token;
            }

        }

        #endregion

    }

}
