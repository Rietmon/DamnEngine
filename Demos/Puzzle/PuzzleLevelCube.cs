using System.Drawing;
using DamnEngine;

namespace Puzzle
{
    public class PuzzleLevelCube : Component
    {
        public bool IsPainted { get; private set; }

        public void Paint()
        {
            IsPainted = true;
            GetComponent<MeshRenderer>().Material.ColorProperty = Color.Aqua;
        }
    }
}