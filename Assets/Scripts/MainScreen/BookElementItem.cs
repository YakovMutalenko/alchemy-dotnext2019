using System.Threading.Tasks;
using Domain.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MainScreen
{
    public class BookElementItem : MonoBehaviour, IPointerDownHandler
    {
        public GameObject floatingElementPrefab;

        private Image _image;
        private Element _element;
        readonly GameManager _gameManager = GameManager.Instance;

        private void Start()
        {
            _image = GetComponentInChildren<Image>();
            _image.sprite = _element.Sprite;

            GetComponentInChildren<Text>().text = _element.Name;
        }

        public void SetUp(Element element)
        {
            _element = element;
        }

        public async void OnPointerDown(PointerEventData eventData)
        {
            if (!_gameManager.CheckAndLockInput()) 
                return;

            _gameManager.AddElementToForge(_element);
            
            var mixElement = _gameManager.EmptyForgeSlot;
            await _gameManager.HandleUiOperation(SendElementToForge(mixElement));
        }
        
        private Task SendElementToForge(ForgeSlot mixElement)
        {
            return Instantiate(floatingElementPrefab, _gameManager.CanvasTransform)
                .GetComponent<FloatingElement>()
                .Run(
                    _image.transform.position,
                    mixElement.transform.position,
                    _element.Sprite,
                    () => mixElement.ChangeSprite(_element.Sprite));
        }
    }
}