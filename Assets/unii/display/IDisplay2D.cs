using UnityEngine;
using System.Collections;

namespace itfantasy.unii
{
    public interface IDisplay2D
    {
        float x { get; set; }
        float y { get; set; }
        float z { get; set; }
        float scaleX { get; set; }
        float scaleY { get; set; }
        float rotationX { get; set; }
        float rotationY { get; set; }
        float rotationZ { get; set; }
        Color color { get; set; }
        float alpha { get; set; }
    }
}
