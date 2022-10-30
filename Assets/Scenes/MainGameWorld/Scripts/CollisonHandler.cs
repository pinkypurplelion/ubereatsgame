using UnityEngine;
using UnityEngine.UI;

namespace Scenes.MainGameWorld.Scripts
{
    /// <summary>
    /// Used to debug if players have collieded with buildings.
    /// </summary>
    /// <author>Alimah Muhammad</author>
    public class CollisonHandler : MonoBehaviour
    {
        [SerializeField] private Text popup;

        // Start is called before the first frame update
        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                Debug.Log("Bumped into building");
            }
        }
    }
}