using System;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace MainScreen
{
    public class NewElementWindow : MonoBehaviour
    {
        public float duration = 0.2f;

        public Text elementName;
        public Text elementDescription;
        public Image elementImage;
        private CanvasGroup _canvasGroup;

        public void Initialize(Sprite elementSprite, string name, int score, string description)
        {
            elementImage.sprite = elementSprite;
            _canvasGroup = GetComponent<CanvasGroup>();
            elementName.text = name + Environment.NewLine + $"{score} {ScoreEndings(score)}";
            elementDescription.text = description;
        }

        public async UniTask Show()
        {
            await AnimationRunner.Run((t) => { _canvasGroup.alpha = t; }, duration);
            
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
        }

        public void Hide()
        {
            Destroy(gameObject);
        }

        private string ScoreEndings(int score)
        {
            var lastTwoNumbers = score % 100;

            switch (lastTwoNumbers)
            {
                case 11:
                case 12:
                case 13:
                case 14:
                    return "очков";
                default:
                {
                    var lastNumber = score % 10;
                    switch (lastNumber)
                    {
                        case 1:
                            return "очко";
                        case 2:
                        case 3:
                        case 4:
                            return "очка";
                        default:
                            return "очков";
                    }
                }
            }
        }
    }
}