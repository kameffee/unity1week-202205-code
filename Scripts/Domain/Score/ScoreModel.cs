using MessagePipe;
using UniRx;
using UnityEngine;

namespace Unity1week202205.Domain.Score
{
    /// <summary>
    /// スコア Model
    /// </summary>
    public class ScoreModel
    {
        public IReadOnlyReactiveProperty<long> Score => _score;
        private readonly ReactiveProperty<long> _score = new();

        private readonly IPublisher<ChangedScore> _scorePublisher;

        public ScoreModel(IPublisher<ChangedScore> scorePublisher)
        {
            _scorePublisher = scorePublisher;
        }

        public void Add(long value)
        {
            var changedScore = new ChangedScore(_score.Value, value);
            _score.Value = changedScore.After;
            _scorePublisher.Publish(changedScore);
            Debug.Log(changedScore);
        }

        public void Reset()
        {
            _score.Value = 0;
        }
    }
}
