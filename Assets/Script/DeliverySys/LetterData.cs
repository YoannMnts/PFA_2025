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
        public string senderName;
        [TextArea]
        public string text;
        [TextArea]
        public string[] sendedText;
        [TextArea]
        public string[] receivedText;

        public int glansGain;
        public int stampsGain;

        public PnjData appearingCharacter;
        public PnjData disappearingCharacter;
    }
}