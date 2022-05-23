using System;
using Spine.Unity;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Unity1week202205.Presentation
{
    /// <summary>
    /// 博士ちゃん表示
    /// </summary>
    public class DoctorView : MonoBehaviour, IDoctorView
    {
        [SerializeField]
        private SkeletonAnimation _skeletonAnimation;

        [SerializeField]
        [SpineAnimation]
        private string _idleState;

        [SerializeField]
        [SpineAnimation]
        private string _eyeBrinkState;
        
        [SerializeField]
        [SpineAnimation]
        private string _smileState;

        [SerializeField]
        [SpineAnimation]
        private string _feverState;
        
        [SerializeField]
        [SpineAnimation]
        private string _ehhenState;
        
        private const int BodyTrack = 0;
        private const int EyeTrack = 1;
        private const int EmotionTrack = 2;
        private const int MotionTrack = 3;

        private void Start()
        {
            Play(DoctorAnimationType.Normal);

            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.Alpha1))
                .Subscribe(_ => Play(DoctorAnimationType.Fever))
                .AddTo(this);
            
            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.Alpha2))
                .Subscribe(_ => Play(DoctorAnimationType.Normal))
                .AddTo(this);
            
            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.Alpha3))
                .Subscribe(_ => Play(DoctorAnimationType.Result))
                .AddTo(this);
        }

        public void Play(DoctorAnimationType doctorAnimationType)
        {
            switch (doctorAnimationType)
            {
                case DoctorAnimationType.Ready:
                case DoctorAnimationType.Normal:
                    _skeletonAnimation.state.SetAnimation(BodyTrack, _idleState, true);
                    _skeletonAnimation.state.SetAnimation(EyeTrack, _eyeBrinkState, true);
                    _skeletonAnimation.state.SetEmptyAnimation(EmotionTrack, 0);
                    _skeletonAnimation.state.SetEmptyAnimation(MotionTrack, 0);

                    break;
                case DoctorAnimationType.Fever:
                    _skeletonAnimation.state.SetAnimation(EyeTrack, _eyeBrinkState, true);
                    _skeletonAnimation.state.SetAnimation(EmotionTrack, _smileState, true);
                    _skeletonAnimation.state.SetAnimation(MotionTrack, _feverState, true);
                    break;
                case DoctorAnimationType.Result:
                    _skeletonAnimation.state.SetAnimation(MotionTrack, _ehhenState, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(doctorAnimationType), doctorAnimationType, null);
            }
        }
    }
}
