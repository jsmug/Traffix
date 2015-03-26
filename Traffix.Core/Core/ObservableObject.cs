using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Traffix.Core
{

    public abstract class ObservableObject : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;


        protected ObservableObject()
        {
        }


        #region protected

        protected virtual void OnPropertyChanged<R>(Expression<Func<ObservableObject, R>> propertyExpression)
        {

            OnPropertyChanged(this.GetPropertySymbol(propertyExpression));

        }

        protected virtual void OnPropertyChanged(string propertyName)
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        #endregion

    }

}
