using UnityEngine;

public class ShipTransformations : MonoBehaviour
{
    public Transform parentInitial;
    public Transform parentNew;

    public Transform part1;
    public Transform cameraChild;
    public Transform part2;
    public Transform wingR;
    public Transform wingL;

    void Start()
    {
        // Apply new transformations
        UpdateTransform(part1, parentInitial, parentNew);
        UpdateTransform(cameraChild, parentInitial, parentNew);
        UpdateTransform(part2, parentInitial, parentNew);
        UpdateTransform(wingR, parentInitial, parentNew);
        UpdateTransform(wingL, parentInitial, parentNew);
    }

    void UpdateTransform(Transform part, Transform parentInitial, Transform parentNew)
    {
        // Calculate the relative position and rotation
        Vector3 relativePos = parentInitial.InverseTransformPoint(part.position);
        Quaternion relativeRot = Quaternion.Inverse(parentInitial.rotation) * part.rotation;

        // Apply new transformations
        part.position = parentNew.TransformPoint(relativePos);
        part.rotation = parentNew.rotation * relativeRot;
    }
}
