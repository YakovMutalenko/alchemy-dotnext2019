using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MainScreen
{
    public class ForgeSlot : MonoBehaviour, IPointerClickHandler
    {
        public float duration = 1f;
        public GameObject floatingElementPrefab;
        public GameObject mixPoint;

        private Image _elementImage;
        private GameManager _gameManager;

        public bool IsEmpty { get; private set; } = true;

        private void Start()
        {
            _gameManager = GameManager.Instance;
            _elementImage = transform.GetChild(0).GetComponent<Image>();
        }

        public async Task ChangeSprite(Sprite sprite)
        {
            _elementImage.sprite = sprite;
            _elementImage.color = Color.white;

            IsEmpty = false;
            await _gameManager.PerformMix();
        }

        public async void OnPointerClick(PointerEventData eventData)
        {
            if (_gameManager.CheckAndLockInput())
            {
                _gameManager.ClearForge();
                await _gameManager.HandleUiOperation(Erase());
            }
        }
        
        public async Task Erase()
        {
            if (!IsEmpty)
            {
                await EraseElementAnimation();
                IsEmpty = true;
            }
        }

        public async Task Mix()
        {
            await MixAnimation();
            IsEmpty = true;
        }
        
        private async Task EraseElementAnimation()
        {
            float t = 0;
            
            var ms = (int)(duration * 50);

            while (t <= 1)
            {
                _elementImage.color = Color.Lerp(Color.white, Color.clear, Mathf.SmoothStep(0f, 1f, t));
                t += 0.05f;
                await Task.Delay(ms);
            }
            
            _elementImage.color = Color.clear;
        }

        private async Task MixAnimation()
        {
            _elementImage.color = Color.clear;

            await Instantiate(floatingElementPrefab, _gameManager.UnderUpperLayerTransform)
                .GetComponent<FloatingElement>()
                .Run(
                    _elementImage.transform.position,
                    mixPoint.transform.position, 
                    _elementImage.sprite, 
                    null);
        }
    }
}