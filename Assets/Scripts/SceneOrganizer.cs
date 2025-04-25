using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class SceneOrganizer : MonoBehaviour
{
    // Categorías principales
    public enum ObjectCategory
    {
        Vehiculos,
        Carreteras,
        Edificios,
        Decoracion,
        Luces,
        Otros
    }

    // Categoría asignada a este objeto
    public ObjectCategory categoria = ObjectCategory.Otros;

    // Método para asignar un objeto a su categoría correspondiente
    public void AsignarACategoria()
    {
        string nombreCategoria = categoria.ToString();
        GameObject contenedor = GameObject.Find(nombreCategoria);
        
        // Si no existe el contenedor, lo creamos
        if (contenedor == null)
        {
            contenedor = new GameObject(nombreCategoria);
        }
        
        // Asignamos este objeto como hijo del contenedor
        transform.SetParent(contenedor.transform);
    }
}

// Extendemos el Editor para agregar botones al componente
[CustomEditor(typeof(SceneOrganizer))]
public class SceneOrganizerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        SceneOrganizer organizador = (SceneOrganizer)target;
        
        if(GUILayout.Button("Asignar a Categoría"))
        {
            organizador.AsignarACategoria();
        }
        
        if(GUILayout.Button("Organizar Todos los Objetos"))
        {
            OrganizarTodos();
        }
    }
    
    // Método para organizar todos los objetos con el componente SceneOrganizer
    void OrganizarTodos()
    {
        SceneOrganizer[] organizadores = FindObjectsOfType<SceneOrganizer>();
        foreach(SceneOrganizer org in organizadores)
        {
            org.AsignarACategoria();
        }
        Debug.Log("Se han organizado " + organizadores.Length + " objetos en sus categorías");
    }
}

// Menú para organizar la escena
public class OrganizarMenu
{
    [MenuItem("Herramientas/Organizar Escena")]
    static void OrganizarEscena()
    {
        // Crear contenedores si no existen
        CrearContenedoresPrincipales();
        
        // Organizar todos los objetos
        SceneOrganizer[] organizadores = Object.FindObjectsOfType<SceneOrganizer>();
        foreach(SceneOrganizer org in organizadores)
        {
            org.AsignarACategoria();
        }
        
        Debug.Log("Escena organizada exitosamente");
    }
    
    // Método para crear los contenedores principales
    static void CrearContenedoresPrincipales()
    {
        string[] categorias = System.Enum.GetNames(typeof(SceneOrganizer.ObjectCategory));
        
        foreach(string categoria in categorias)
        {
            if(GameObject.Find(categoria) == null)
            {
                new GameObject(categoria);
                Debug.Log("Contenedor creado: " + categoria);
            }
        }
    }
}
#endif