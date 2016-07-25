using CSharp.Meta;
using System.ComponentModel;

namespace BikeAround.App.Meta
{
    internal abstract class NotifyPropertyChangesTrait : Trait, INotifyPropertyChanging, INotifyPropertyChanged, INotifyPropertyChangesEx
    {
        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanging(string propertyName)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}