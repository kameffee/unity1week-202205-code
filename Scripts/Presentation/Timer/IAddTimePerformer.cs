using System;

namespace Unity1week202205.Presentation
{
    /// <summary>
    /// 時間追加演出
    /// </summary>
    public interface IAddTimePerformer
    {
        void Play(TimeSpan time);
    }
}
