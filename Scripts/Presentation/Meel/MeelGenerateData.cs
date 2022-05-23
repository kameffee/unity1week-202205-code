using System;
using UnityEngine;

namespace Unity1week202205.Presentation.Meel
{
    /// <summary>
    /// ミールを生成するときに必要な情報
    /// </summary>
    [Serializable]
    public class MeelGenerateData
    {
        public Transform GeneratePoint;
        public Transform Holder;
        public MeelView MeelPrefab;
        public MeelView BigMeelPrefab;
    }
}
