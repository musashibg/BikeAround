namespace BikeAround.App.Meta
{
    internal interface INotifyPropertyChangesEx
    {
        void RaisePropertyChanging(string propertyName);

        void RaisePropertyChanged(string propertyName);
    }
}