using UnityEngine;

public interface IUIElement
{
    void SetText(string text);
    void CreateElement();
    public GameObject GetGameObject();
    void Pressed(bool correctInput);
    bool isPressed { get; }

}