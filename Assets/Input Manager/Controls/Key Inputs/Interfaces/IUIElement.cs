using UnityEngine;

public interface IUIElement
{
    void SetText(string text);
    void CreateElement();
    void DestroyElement();
    public GameObject GetGameObject();
    void Pressed();
    
}