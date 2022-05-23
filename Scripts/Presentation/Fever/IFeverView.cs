namespace Unity1week202205.Presentation.Fever
{
    public interface IFeverView
    {
        void Render(float value);

        void FeverStart();

        void FeverStop();
    }
}
