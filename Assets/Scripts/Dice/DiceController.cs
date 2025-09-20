using UnityEngine;

public interface IDice
{
    int Roll();
}


public class DiceController : MonoBehaviour, IDice
{
    private const int MinFace = 1;
    private const int MaxFace = 6;

    public int Roll()
    {
        return Random.Range(MinFace, MaxFace + 1);
    }
}