using UnityEngine;

namespace Scenes.MainGameWorld.Scripts
{
    public class BuildingTile : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            int building = Random.Range(0, 4);
            transform.Find($"building{building}").gameObject.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
