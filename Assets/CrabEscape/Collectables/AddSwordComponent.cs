using UnityEngine;

public class AddSwordComponent : MonoBehaviour
{
    [SerializeField] private int _swordCount;
    private Hero _hero;

    private void Start()
    {
        _hero = FindObjectOfType<Hero>();
    }

    public void AddSword()
    {
        _hero.SwordCounter(_swordCount);
    }
}
