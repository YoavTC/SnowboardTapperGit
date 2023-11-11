using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HelperFunctions : MonoBehaviour
{
    #region Unity Runtime Related

    private static PointerEventData eventDataCurrentPos;
    private static List<RaycastResult> results;
    /// <summary>
    /// Check if the mouse is over a UI element
    /// </summary>
    public static bool IsOverUI()
    {
        eventDataCurrentPos = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPos, results);
        return results.Count > 0;
    }
    
    private static Dictionary<float, WaitForSeconds> waitDictionary = new Dictionary<float, WaitForSeconds>();
    /// <summary>
    /// Returns a new WaitForSeconds if one has already been made, if not, creates new one
    /// </summary>
    /// <param name="time"></param>
    public static WaitForSeconds GetWait(float time)
    {
        if (waitDictionary.TryGetValue(time, out var wait)) return wait;

        waitDictionary[time] = new WaitForSeconds(time);
        return waitDictionary[time];
    }

    public static Transform GetRandomObject(List<Transform> list, Transform avoidedObject = null)
    {
        Transform randomObject = list[Random.Range(0, list.Count)];

        if (avoidedObject != null && randomObject == avoidedObject)
        {
            return GetRandomObject(list, avoidedObject);
        }

        return randomObject;
    }

    #endregion

    #region Children Related

    /// <summary>
    /// Destroy all children under a given Transform parent, if tag given,
    /// only children with tag will be destroyed. If the destroyWithoutTag parameter is true,
    /// all children WITHOUT said tag will get destroyed
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="tag"></param>
    /// <param name="destroyWithoutTag"></param>
    public static void DestroyChildren(Transform parent, string tag = "", bool destroyWithoutTag = false)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            GameObject child = parent.GetChild(i).gameObject;
            bool hasTag = child.CompareTag(tag);

            if ((!destroyWithoutTag && hasTag) || (destroyWithoutTag && !hasTag) || tag == "")
            {
                Destroy(child);
            }
        }
    }

    /// <summary>
    /// Returns the first Transform child with a certain tag
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="tag"></param>
    public static Transform GetFirstChildWithTag(Transform parent, string tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).CompareTag(tag)) return parent.GetChild(i);
        }

        return null;
    }

    /// <summary>
    /// Returns a List of Transform children from a parent
    /// </summary>
    /// <param name="parent"></param>
    public static List<Transform> GetChildren(Transform parent)
    {
        List<Transform> newList = new List<Transform>();
        for (int i = 0; i < parent.childCount; i++)
        {
            newList.Add(parent.GetChild(i));
        }

        return newList;
    }

    /// <summary>
    /// Returns a List of transform children with a certain tag from a List of Transform objects
    /// </summary>
    /// <param name="list"></param>
    /// <param name="tag"></param>
    public static List<Transform> GetTransformsWithTag(List<Transform> list, string tag)
    {
        List<Transform> newList = new List<Transform>();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].CompareTag(tag)) newList.Add(list[i]);
        }

        return newList;
    }
    
    /// <summary>
    /// Returns a List of transform children with a certain tag from a Transform parent
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="tag"></param>
    public static List<Transform> GetChildrenWithTag(Transform parent, string tag)
    {
        List<Transform> newList = new List<Transform>();
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).CompareTag(tag)) newList.Add(parent.GetChild(i));
        }

        return newList;
    }
    
    /// <summary>
    /// Returns a List of Transform objects with a certain component attached to them, from a List of Transform objects
    /// </summary>
    /// <param name="list"></param>
    /// <typeparam name="T"></typeparam>
    public static List<Transform> GetTransformsWithComponent<T>(List<Transform> list) where T : Component
    {
        List<Transform> newList = new List<Transform>();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].GetComponent<T>())
            {
                newList.Add(list[i]);
            }
        }

        return newList;
    }
    
    /// <summary>
    /// Returns a List of Transform CHILD objects with a certain component attached to them, from a singular parent
    /// </summary>
    /// <param name="parent"></param>
    /// <typeparam name="T"></typeparam>
    public static List<Transform> GetChildrenWithComponent<T>(Transform parent) where T : Component
    {
        List<Transform> newList = new List<Transform>();
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).GetComponent<T>())
            {
                newList.Add(parent.GetChild(i));
            }
        }

        return newList;
    }

    #endregion
    
    #region Math & Time Related

    /// <summary>
    /// Get a brand unique new ID
    /// </summary>
    public static string GetUUID()
    {
        Guid uuid = Guid.NewGuid();
        return uuid.ToString();
    }
    
    /// <summary>
    /// Get offset unix time stamp, if empty returns current
    /// </summary>
    /// <param name="hoursOffset"></param>
    public static long GetUnixTime(float hoursOffset = 0f)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        DateTimeOffset future = now.AddHours(hoursOffset);
        return future.ToUnixTimeSeconds();
    }

    /// <summary>
    /// Returns a given Quaternion's reversed counterpart
    /// </summary>
    /// <param name="quaternion"></param>
    public static Quaternion ReverseQuaternion(Quaternion quaternion)
    {
        return new Quaternion(-quaternion.x, -quaternion.y, -quaternion.z, quaternion.w);
    }

    #endregion

    #region UI Related

    /// <summary>
    /// Fade a UI element from its current alpha value to another over a period of time
    /// </summary>
    /// <param name="uiElement"></param>
    /// <param name="targetAlpha"></param>
    /// <param name="duration"></param>
    public static IEnumerator FadeUIElement(Graphic uiElement, float targetAlpha, float duration)
    {
        float startAlpha = uiElement.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            uiElement.color = new Color(uiElement.color.r, uiElement.color.g, uiElement.color.b, newAlpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        uiElement.color = new Color(uiElement.color.r, uiElement.color.g, uiElement.color.b, targetAlpha);
    }

    /// <summary>
    /// Shakes the given camera
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="duration"></param>
    /// <param name="magnitude"></param>
    /// <param name="returnToOrigin"></param>
    /// <returns></returns>
    private static bool isShaking;
    public static IEnumerator ScreenShake(Camera camera, float duration, float magnitude, bool returnToOrigin)
    {
        Debug.Log("Shaking screen");
        if (isShaking)
        {
            yield break;
        }
        
        Vector3 originalPosition = camera.transform.position;
        isShaking = true;
        
        while (duration > 0)
        {
            float x = originalPosition.x + (Random.Range(-1f, 1f) * magnitude);
            float y = originalPosition.y + (Random.Range(-1f, 1f) * magnitude);
            
            camera.transform.position += new Vector3(x, y, 0f);
            duration -= Time.deltaTime;
            yield return null;
            camera.transform.position = originalPosition;
        }
        
        if (returnToOrigin && camera.transform.position.x != originalPosition.x)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, originalPosition, 0.5f);
            yield return null;
        }
        
        camera.transform.position = originalPosition;
        isShaking = false;
    }

    #endregion
}
