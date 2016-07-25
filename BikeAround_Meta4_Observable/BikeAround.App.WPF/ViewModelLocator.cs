using BikeAround.App.ViewModels;

namespace BikeAround.App
{
    public static class ViewModelLocator
    {
        public static MainViewModel MainViewModel { get; } = new MainViewModel();
    }
}