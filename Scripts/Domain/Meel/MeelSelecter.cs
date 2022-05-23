using System.Collections.Generic;
using System.Linq;

namespace Unity1week202205.Domain.Meel
{
    /// <summary>
    /// ミールの選択状態管理
    /// </summary>
    public class MeelSelecter
    {
        /// <summary>
        /// 選択中のミールの数
        /// </summary>
        public int Count => _stack.Count;

        /// <summary>
        /// 最初に選択したミール
        /// </summary>
        public MeelModel First => _stack.FirstOrDefault();

        /// <summary>
        /// 最後に選択されたミール
        /// </summary>
        public MeelModel Last => _stack.Peek();

        private readonly Stack<MeelModel> _stack = new();

        public MeelSelecter()
        {
        }

        public IEnumerable<MeelModel> All() => _stack;

        /// <summary>
        /// 1つも選択してない状態か
        /// </summary>
        public bool IsEmpty() => !_stack.Any();

        /// <summary>
        /// 1つ以上選択している状態か
        /// </summary>
        public bool Any() => _stack.Any();

        /// <summary>
        /// 選択状態へ登録
        /// </summary>
        /// <param name="meelModel"></param>
        public void Register(MeelModel meelModel)
        {
            // 選択状態へ変更
            meelModel.Select(new MeelSelectedNumber(_stack.Count + 1));

            meelModel.SetIsLastSelected(true);

            if (_stack.TryPeek(out var last))
            {
                last.SetIsLastSelected(false);
            }

            // 登録
            _stack.Push(meelModel);
        }

        /// <summary>
        /// 選択解除
        /// </summary>
        /// <param name="meelModel"></param>
        public void UnRegister(MeelModel meelModel)
        {
            for (int i = 0; i < _stack.Count; i++)
            {
                var meel = _stack.Pop();
                meel.Deselect();
                meel.SetIsLastSelected(false);
                if (meel.Id == meelModel.Id)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// リセット
        /// </summary>
        public void Reset()
        {
            foreach (var meelModel in _stack)
            {
                // 選択解除
                meelModel.Deselect();
                meelModel.SetIsLastSelected(false);
            }

            _stack.Clear();
        }

        /// <summary>
        /// 選択されているビッグミールの個数
        /// </summary>
        /// <returns></returns>
        public int BigMeelCount()
        {
            return _stack.Count(model => model.Size.IsBig());
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Clear()
        {
            _stack.Clear();
        }
    }
}
