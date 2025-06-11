using BepInEx;
using BepInEx.Unity.IL2CPP;
using Il2CppSystem.Threading;
using Locomotion;
using Lolopupka;
using UnityEngine;
using UnityEngine.XR;

namespace CapuchinTemplate
{
    [BepInPlugin("instel.capucam", "Capucam", "1.0.0")]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            AddComponent<PluginBehaviour>();
        }
    }

    public class PluginBehaviour : MonoBehaviour
    {
        GameObject box;
        GameObject dirsphere;
        bool leftSecondary;

        void Update()
        {
            leftSecondary = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand)
                .TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryl) && secondaryl;

            Camcorder.camcorderInstance.SetCamerasFOV(80f);
            Camcorder.camcorderInstance.camcorderCamera.nearClipPlane = 0.0f;

            if (box == null)
            {
                box = GameObject.CreatePrimitive(PrimitiveType.Cube);
                box.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Color");
                box.GetComponent<Renderer>().material.color = Color.black;
                box.GetComponent<BoxCollider>().enabled = false;
                box.transform.localScale = Vector3.one * 0.1f;
            }

            if (dirsphere == null)
            {
                dirsphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                dirsphere.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Color");
                dirsphere.GetComponent<Renderer>().material.color = Color.white;
                dirsphere.GetComponent<SphereCollider>().enabled = false;
                dirsphere.transform.localScale = Vector3.one * 0.05f;
            }

            if (leftSecondary)
            {
                box.transform.position = Player.Instance.RightHand.transform.position;
                box.transform.rotation = Player.Instance.RightHand.transform.rotation * Quaternion.Euler(180f, 90f, 90f);
            }

            Camcorder.camcorderInstance.camcorderCamera.transform.position = dirsphere.transform.position;
            Camcorder.camcorderInstance.camcorderCamera.transform.rotation = dirsphere.transform.rotation;
            dirsphere.transform.position = box.transform.position + (box.transform.forward * 0.05f);
            dirsphere.transform.rotation = box.transform.rotation;

            box.SetActive(true);
            dirsphere.SetActive(true);
        }
    }
}
