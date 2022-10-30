using UnityEngine;
using UnityEngine.UIElements;

namespace Scenes.MainGameWorld.Scripts
{
    /// <summary>
    /// A base class to build UI managers.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        // Base UI Elements
        private UIDocument _uiDocument;
        protected VisualElement RootVisualElement;

        protected void Awake()
        {
            _uiDocument = transform.GetComponent<UIDocument>();
            RootVisualElement = _uiDocument.rootVisualElement;
        }
    }
}