namespace Unity1week202205.Presentation
{
    public interface IInputHandler
    {
        bool IsActive { get; }

        void SetActive(bool isActive);
    }
}