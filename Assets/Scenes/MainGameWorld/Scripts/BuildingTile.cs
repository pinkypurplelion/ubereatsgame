using UnityEngine;

namespace Scenes.MainGameWorld.Scripts
{
    /// <summary>
    /// Used to manage the building tile objects in the city
    /// </summary>
    public class BuildingTile : MonoBehaviour
    {
        /// <summary>
        /// Used to randomly select the building model to use.
        /// </summary>
        void Start()
        {
            int building = Random.Range(0, 4);
            transform.Find($"building{building}").gameObject.SetActive(true);
        }
    }
}