using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float _speed;
    private float _directionX;
    private float _directionY;

    public void SetDirection(float dirX, float dirY)
    {
        _directionX = dirX;
        _directionY = dirY;
    }

    private void Update()
    {       
        var newXPosition = transform.position.x + _directionX * _speed * Time.deltaTime;
        var newYPosition = transform.position.y + _directionY * _speed * Time.deltaTime;
        transform.position = new Vector2(newXPosition, newYPosition);        
    }
}
