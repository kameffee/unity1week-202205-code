using System;
using UnityEngine;

namespace Unity1week202205.Presentation
{
    /// <summary>
    /// 時間追加演出
    /// </summary>
    public class AddTimePerformer : MonoBehaviour, IAddTimePerformer
    {
        [SerializeField]
        private AddTimeView _view;

        public void Play(TimeSpan time)
        {
            _view.Play(time);
        }
    }
}
