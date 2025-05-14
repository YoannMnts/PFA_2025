using UnityEngine;

namespace Script.DeliverySys
{
    [CreateAssetMenu(fileName = "NewPnj", menuName = "Hazel/Pnj", order = 0)]
    public class PnjData : ScriptableObject
    {
        public string name;
        public Vector3 mapPosition;
        public Vector3 position;
    }
}