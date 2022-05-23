namespace Unity1week202205.Presentation.Combo
{
    /// <summary>
    /// コンボ表示
    /// </summary>
    public interface IComboView
    {
        void Render(int combo);

        void RenderGauge(float normalize);
    }
}
