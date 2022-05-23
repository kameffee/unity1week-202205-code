using System;
using TMPro;
using UnityEngine;

namespace Unity1week202205.Presentation
{
    /// <summary>
    /// 現在のチェーン数表示
    /// </summary>
    public class MeelSelectCountView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro _count;

        public void RenderNumber(int number)
        {
            gameObject.SetActive(true);
            _count.SetText(number.ToString());
        }

        /// <summary>
        /// 非表示
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            // 常に読める向きに更新
            transform.rotation = Quaternion.identity;
        }
    }
}
