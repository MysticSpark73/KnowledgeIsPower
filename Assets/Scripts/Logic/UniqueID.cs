using UnityEngine;

namespace Logic
{
    public class UniqueID : MonoBehaviour
    {
        public string ID => _id;
        private string _id;
        
        public void SetId(string id) => _id = id;
    }
}