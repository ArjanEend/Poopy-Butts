using UnityEngine;
using System.Collections;

public class Iktest : MonoBehaviour
{
    public Transform Root;
    public Transform Mid;
    public Transform End;

    public Transform MidIK;
    public Transform EndIK;

    Quaternion RootToMidQuat = Quaternion.identity;
    Quaternion RootToEndQuat = Quaternion.identity;

    void LateUpdate()
    {
        float RootToMid = Vector3.Distance(Root.position, Mid.position);
        float MidToEnd = Vector3.Distance(Mid.position, End.position);
        float RootToEnd = Vector3.Distance(EndIK.position, Root.position); //total leg length  

        RootToEndQuat.SetLookRotation(EndIK.position - Root.position, MidIK.position - Root.position);

        if (Mathf.Abs(RootToMid - MidToEnd) > RootToEnd)
        {
            Mid.localEulerAngles = new Vector3(Mid.localEulerAngles.x, Mid.localEulerAngles.y, 180);
            RootToMidQuat.eulerAngles = new Vector3(0, -270, 0); //-270 determines direction Knee will face
        }
        else if (Mathf.Abs(RootToMid + MidToEnd) < RootToEnd)
        {
            Mid.localEulerAngles = new Vector3(Mid.localEulerAngles.x, Mid.localEulerAngles.y, 0);
            RootToMidQuat.eulerAngles = new Vector3(0, -270, 0);
        }
        else
        {
            //uses pythagorean theroem to figure out angles
            float modz = Mathf.Asin((RootToMid * RootToMid + MidToEnd * MidToEnd - RootToEnd * RootToEnd) / (2 * RootToMid * MidToEnd)) * Mathf.Rad2Deg + 90;
            Mid.localEulerAngles = new Vector3(Mid.localEulerAngles.x, Mid.localEulerAngles.y, modz);
            RootToMidQuat.eulerAngles = new Vector3(0, -270, Mathf.Asin((RootToEnd * RootToEnd + RootToMid * RootToMid - MidToEnd * MidToEnd) / (2 * RootToEnd * RootToMid)) * Mathf.Rad2Deg - 90);
        }
        Root.rotation = RootToEndQuat * RootToMidQuat; //quaternion multiplication will make RootToMidQuat a child to the rotations of RootToEndQuat      
        End.rotation = EndIK.rotation;
    }
}
