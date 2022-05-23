using System;
using Spine.Unity;
using UnityEngine;

namespace Unity1week202205.Data
{
    /// <summary>
    /// ミールプロファイル
    /// </summary>
    [Serializable]
    public class MeelProfile
    {
        [SerializeField]
        private int _typeId;
        
        [SerializeField]
        private Color _color = Color.white;

        [SerializeField]
        private string _skinName;

        /// <summary>
        /// ミールタイプ (色)
        /// </summary>
        public int TypeId => _typeId;
        
        /// <summary>
        /// 色
        /// </summary>
        public Color Color => _color;
        
        /// <summary>
        /// Spineで使うSkin名
        /// </summary>
        public string SkinName => _skinName;
    }
}
