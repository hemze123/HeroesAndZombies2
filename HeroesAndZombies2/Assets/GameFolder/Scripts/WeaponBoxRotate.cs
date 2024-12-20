using UnityEngine;

public class WeaponBoxRotate : MonoBehaviour
{
    // Nesnenin dönüş hızı (derece/saniye)
    public Vector3 rotationSpeed = new Vector3(0, 100, 0);

    void Update()
    {
        // Nesneyi belirlenen hızda döndür
     //   transform.Rotate(rotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up, 100 * Time.deltaTime);
    }
}
