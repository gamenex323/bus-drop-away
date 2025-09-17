using UnityEngine;
using static GameManager;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameMode currentMode = GameMode.Easy;
    public ColorHolder[] colors;
    void Start()
    {
        if (instance == null)
            instance = this;
    }

    public Material GetColor(ColorData type)
    {
        for (int i = 0; i < colors.Length; i++)
        {
            if (type == colors[i].colorType)
            {
                return colors[i].colorMaterial;
            }
        }
        return null;
    }



}
public enum ColorData
{
    Red,
    Purple,
    Blue,
    Green,
    Yellow
}
public enum GameMode { Easy, Medium, Hard }

[System.Serializable]
public class ColorHolder
{
    public ColorData colorType;
    public Material colorMaterial;
}
