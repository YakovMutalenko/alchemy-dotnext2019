using System.Threading.Tasks;
using Domain;
using UnityEngine;

namespace ElementsBook
{
    public class GameManager : MonoBehaviour
    {
        public MixElement mixElementOne;
        public MixElement mixElementTwo;
        public GameObject canvas;
        public GameObject elementItemPrefab;
        public GameObject elementsBook;
        private IReceiptsBook _receiptsBook;
        private IForge _forge;
        
        public static GameManager Instance { get; private set; }

        private bool _inputLocked;

        public bool CheckAndLockInput()
        {
            if (_inputLocked) 
                return false;
            
            _inputLocked = true;
            return true;
        }

        public Transform CanvasTransform => canvas.transform;

        private void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            _receiptsBook = new ReceiptsBook();
            _forge = new Forge(_receiptsBook, new DummyMixChecker());
            InitializeElements();
        }

        public MixElement GetMixElement()
        {
            return mixElementOne.IsEmpty ? mixElementOne : mixElementTwo;
        }

        public async Task PerformMix()
        {
            var resultTask = _forge.GetMixResult();
            if (resultTask == null)
                return;
            
            var mixResult = await resultTask;
            if (mixResult.IsSuccess /*mixResult.IsNewlyCreated*/)
            {
                await Task.WhenAll(mixElementOne.Mix(), mixElementTwo.Mix());

                if (mixResult.IsNewlyCreated)
                {
                    AddNewElement(mixResult.Element);
                }
            }
            else
                await mixElementTwo.Erase();
        }

        public async Task HandleUiOperation(Task uiOperation)
        {
            _inputLocked = true;
            await uiOperation;
            _inputLocked = false;
        }

        private void AddNewElement(Element element)
        {
            Instantiate(elementItemPrefab, elementsBook.transform)
                .GetComponent<ElementItem>()
                .SetUp(element);
        }
        
        private void InitializeElements()
        {
            var openedElements = _receiptsBook.GetOpenedElements();

            foreach (var element in openedElements)
            {
                Instantiate(elementItemPrefab, elementsBook.transform)
                    .GetComponent<ElementItem>()
                    .SetUp(element);
            }
        }

        public void AddElementToForge(Element element)
        {
            _forge.AddElement(element);
        }

        public void ClearForge()
        {
            _forge.Clear();
        }
    }
}