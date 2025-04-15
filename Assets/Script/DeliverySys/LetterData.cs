using UnityEngine;

namespace Script.DeliverySys
{
    [CreateAssetMenu(fileName = "NewLetter", menuName = "Hazel/Letter", order = 0)]
    public class LetterData : ScriptableObject
    {
        public PnjData sender;
        public PnjData receiver;
        [TextArea]
        public string text;
    }
}