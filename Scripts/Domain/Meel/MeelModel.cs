using System;
using Cysharp.Threading.Tasks;
using UniRx;
using Unity1week202205.Data;
using UnityEngine;

namespace Unity1week202205.Domain.Meel
{
    /// <summary>
    /// ミール
    /// </summary>
    public class MeelModel : IDisposable
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id => _id;

        /// <summary>
        /// タイプ
        /// </summary>
        public int TypeId => _profile.TypeId;

        /// <summary>
        /// サイズ
        /// </summary>
        public MeelSize Size => _size;

        /// <summary>
        /// 色
        /// </summary>
        public Color Color => _profile.Color;

        /// <summary>
        /// 座標
        /// </summary>
        public Vector3 Position => _position;

        public MeelProfile MeelProfile { get; }

        /// <summary>
        /// 選択状態
        /// </summary>
        public IReadOnlyReactiveProperty<bool> Selected => _selected;

        private readonly ReactiveProperty<bool> _selected = new();

        public IObservable<float> OnBreak => _onBreak;
        private readonly Subject<float> _onBreak = new();

        public IReadOnlyReactiveProperty<bool> IsFreeze => _isFreeze;
        private readonly ReactiveProperty<bool> _isFreeze = new();

        public IReadOnlyReactiveProperty<bool> IsLastSelected => _isLastSelected;
        private readonly ReactiveProperty<bool> _isLastSelected = new();

        public IObservable<Unit> OnSpine => _onSpine;
        private readonly Subject<Unit> _onSpine = new();

        /// <summary>
        /// 何番目に選択されたか
        /// </summary>
        public MeelSelectedNumber SelectedNumber => _selectedNumber;
        private MeelSelectedNumber _selectedNumber = MeelSelectedNumber.None;

        private Subject<Unit> _onCompleteBreak = new();

        private Vector3 _position;
        private readonly int _id;
        private readonly MeelProfile _profile;
        private readonly MeelSize _size;

        public MeelModel(int id, MeelProfile profile, MeelSize size)
        {
            _id = id;
            _profile = profile;
            _size = size;
            MeelProfile = profile;
        }

        public void SetPosition(Vector3 position)
        {
            _position = position;
        }

        public async UniTask Break(float delay)
        {
            _isFreeze.Value = true;
            _onBreak.OnNext(delay);
            await _onCompleteBreak.ToUniTask(true);
        }

        public void OnCompleteBreak() => _onCompleteBreak.OnNext(Unit.Default);

        public void Select(MeelSelectedNumber number)
        {
            _selectedNumber = number;
            _selected.Value = true;
        }

        public void Deselect()
        {
            _selected.Value = false;
            _selectedNumber = MeelSelectedNumber.None;
        }

        public void SetIsLastSelected(bool isNow) => _isLastSelected.Value = isNow;

        public void Freeze() => _isFreeze.Value = true;

        public void UnFreeze() => _isFreeze.Value = false;

        /// <summary>
        /// 回転させる
        /// </summary>
        public void Spine() => _onSpine.OnNext(Unit.Default);

        public void Dispose()
        {
            _selected?.Dispose();
            _onBreak?.Dispose();
            _isFreeze?.Dispose();
            _isLastSelected?.Dispose();
            _onCompleteBreak?.Dispose();
        }
    }
}
