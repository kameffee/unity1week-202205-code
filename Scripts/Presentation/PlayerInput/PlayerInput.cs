using System.Linq;
using Unity1week202205.Domain;
using Unity1week202205.Domain.Meel;
using UnityEngine;
using VContainer.Unity;

namespace Unity1week202205.Presentation
{
    public class PlayerInput : ITickable, IInputHandler
    {
        private readonly InGameService _inGameService;
        private readonly Camera _camera;
        private readonly MeelSelecter _meelSelecter;
        private readonly SpinService _spinService;

        public bool IsActive => _isActive;

        private bool _isActive;

        public PlayerInput(
            InGameService inGameService,
            Camera mainCamera,
            MeelSelecter meelSelecter,
            SpinService spinService)
        {
            _inGameService = inGameService;
            _camera = mainCamera;
            _meelSelecter = meelSelecter;
            _spinService = spinService;
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
        }

        public void Tick()
        {
            if (!_isActive)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                if (_meelSelecter.Any())
                {
                    // 最後のミールから現在のカーソル位置を繋ぐ線
                    var last = _meelSelecter.Last;
                    Debug.DrawLine(last.Position, _camera.ScreenToWorldPoint(Input.mousePosition), Color.magenta);
                }

                _inGameService.Tapping(Input.mousePosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                _inGameService.TapUp();
            }

            if (_meelSelecter.Any())
            {
                var modelArray = _meelSelecter.All().ToArray();
                for (var i = 1; i < modelArray.Length; i++)
                {
                    var prev = modelArray[i - 1];
                    var current = modelArray[i];
                    Debug.DrawLine(prev.Position, current.Position, Color.red);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _spinService.Spin();
            }
        }
    }
}
