using UnityEngine;

namespace Script.Transition
{
    public enum DestinationTag
    {
        Enter,
        A,
        B,
        C
    }
    
    public class TranstionDestination : MonoBehaviour
    {
        public DestinationTag destinationTag;
    }
}
