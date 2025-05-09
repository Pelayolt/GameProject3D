using UnityEngine;

public class ItemRespawnPlatform : MonoBehaviour
{
    public float respawnTime = 30f;
    private GameObject item;
    private float timer = 0f;
    private bool isWaiting = false;

    private void Start()
    {
        // Asume que el arma es el primer hijo de esta plataforma
        if (transform.childCount > 0)
            item = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (item == null) return;

        if (!item.activeSelf && !isWaiting)
        {
            // El arma fue recogida, empezamos a contar
            isWaiting = true;
            timer = 0f;
        }

        if (isWaiting)
        {
            timer += Time.deltaTime;

            if (timer >= respawnTime)
            {
                // Reactivamos el arma y reiniciamos el contador
                item.SetActive(true);
                isWaiting = false;
            }
        }
    }
}
