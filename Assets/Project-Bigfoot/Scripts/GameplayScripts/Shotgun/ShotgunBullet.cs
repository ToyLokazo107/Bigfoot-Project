using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    public float speed = 60f;
    public float lifeTime = 3f;

    private int damage = 10;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        BigfootHealth bigfoot = other.GetComponentInParent<BigfootHealth>();
        Debug.Log("Hit: " + other.name + " Parent: " + other.transform.root.name);

        if (bigfoot != null)
        {
            bigfoot.TakeDamage(damage);
            Debug.Log("Bigfoot hit");
        }

        Destroy(gameObject);
    }
}