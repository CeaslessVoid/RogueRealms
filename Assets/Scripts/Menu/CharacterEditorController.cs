using UnityEngine;

namespace RogueRealms
{
    public class CharacterEditorController : MonoBehaviour
    {
        public GameObject classSelectionRoot;
        public GameObject editorRoot;
        public HumanoidBodyDrawer drawer;

        static readonly Direction[] SpinOrder = { Direction.South, Direction.East, Direction.North, Direction.West };
        int spinIndex;

        public void Open()
        {
            classSelectionRoot.SetActive(false);
            editorRoot.SetActive(true);
        }

        public void Exit()
        {
            editorRoot.SetActive(false);
            classSelectionRoot.SetActive(true);
        }

        public void Spin()
        {
            spinIndex = (spinIndex + 1) % SpinOrder.Length;
            drawer.SetFacing(SpinOrder[spinIndex]);
        }
    }
}
