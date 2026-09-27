using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Turnierprogramm2.ViewModel
{
    [Serializable]
    public abstract class BaseViewModel : INotifyPropertyChanged
    {


        #region NotifyPropertyChanged
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

    }

}
