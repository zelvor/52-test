using UnityEngine;

public interface IDice
{
    int Roll();
}

public class Dice : MonoBehaviour, IDice
{
    private const int Min = 1;
    private const int Max = 6;

    public int Roll()
    {
        return Random.Range(Min, Max + 1);
    }
}