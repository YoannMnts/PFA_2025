using UnityEngine;
using UnityEngine.Serialization;

namespace Script.DeliverySys
{
    [CreateAssetMenu(fileName = "NewLetter", menuName = "Hazel/Letter", order = 0)]
    public class LetterData : ScriptableObject
    {
        public PnjData sender;
        public PnjData receiver;
        public LetterData[] dependencies;
        [TextArea]
        public string text;
        [TextArea]
        public string interactionText;

        public int glansGain;
        public int stampsGain;
    }
}