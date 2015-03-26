using System;

namespace Traffix.Mapping
{

    public class IdentificationToken
    {

        private Guid baseToken;


        public IdentificationToken()
        {
            this.baseToken = Guid.NewGuid();
        }


        #region public

        public bool Equals(IdentificationToken other)
        {

            bool equals = false;

            if (other != null)
            {
                equals = Equals(this.baseToken, other.Token);
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
            return this.baseToken.GetHashCode();
        }

        #endregion

        #region internal 

        internal Guid Token
        {

            get
            {
                return this.baseToken;
            }

        }

        #endregion

    }

}
