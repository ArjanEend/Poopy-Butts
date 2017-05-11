using RocketWorks.Entities;

namespace PoopyButts.Components
{
    public partial class Tilemap : IComponent
    {
        public int[,] tiles;
        public string assetId = "newtiles";
        public float tileSize = 1f;
    }
}
