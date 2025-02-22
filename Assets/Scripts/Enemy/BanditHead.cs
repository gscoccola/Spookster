using UnityEngine;

public class BanditHead : MonoBehaviour
{

    [SerializeField] GameObject bandit;

    public void DecreaseHealthPoints(int amount)
    {
        bandit.GetComponent<Health>().DecreaseHealthPoints(amount * 6);
    }
}
