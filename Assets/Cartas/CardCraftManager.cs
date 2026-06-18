using UnityEngine;

public class CraftManager : MonoBehaviour
{
    public static CraftManager Instance;

    public GameObject waterCard;

    void Awake()
    {
        Instance = this;
    }

    public void TryCraft(
        CardType a,
        CardType b
    )
    {
        bool waterRecipe =
            (a == CardType.Hydrogen &&
             b == CardType.Oxygen)
            ||
            (a == CardType.Oxygen &&
             b == CardType.Hydrogen);

        if (waterRecipe)
        {
            Debug.Log("CRAFTED WATER");

            waterCard.SetActive(true);
        }
    }
}