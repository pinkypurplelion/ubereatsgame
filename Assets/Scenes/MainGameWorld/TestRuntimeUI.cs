using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestRuntimeUI : MonoBehaviour
{
    private Button _button;
    private Label _label;

    private int _clickCount;

    //Add logic that interacts with the UI controls in the `OnEnable` methods
    private void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();

        _button = uiDocument.rootVisualElement.Q<Button>();
        _label = uiDocument.rootVisualElement.Q<Label>();

        _button.RegisterCallback<ClickEvent>(PrintClickMessage);
    }

    private void OnDisable()
    {
        _button.UnregisterCallback<ClickEvent>(PrintClickMessage);
    }

    private void PrintClickMessage(ClickEvent evt)
    {
        ++_clickCount;
        
        var button = evt.currentTarget as Button;
        _label.text = $"{button.text} clicked {_clickCount} times";
    }
}
