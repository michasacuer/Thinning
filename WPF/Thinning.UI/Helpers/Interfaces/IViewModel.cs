namespace Thinning.UI.Helpers.Interfaces
{
    using Caliburn.Micro;

    public interface IViewModel<T>
        where T : IScreen
    {
        void SetReferenceToViewModel(T viewModel);
    }
}
