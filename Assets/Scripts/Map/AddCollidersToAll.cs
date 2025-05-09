using UnityEngine;

public class AddCollidersToAll : MonoBehaviour
{
    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Collider>() == null)
            {
                // Agregamos un BoxCollider
                BoxCollider collider = child.gameObject.AddComponent<BoxCollider>();

                // Ajustamos el collider al tamaño del objeto visual (si tiene un MeshRenderer)
                MeshRenderer renderer = child.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    collider.center = renderer.bounds.center - child.position;
                    collider.size = renderer.bounds.size;
                }
            }
        }
    }
}
