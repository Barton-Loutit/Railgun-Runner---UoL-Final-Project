/*
 * This script is an editor extension which adds a context item for game objects 
 * to cppy the tag of a parent gameObject to all children of the parent.
 * 
 * This script offers no functionality to the game, and was copied from a Unity
 * discussion form by user "Johannski":
 * https://discussions.unity.com/t/is-there-an-easy-way-to-apply-the-same-tag-to-all-children-of-an-object/28582
 * 
 * 
 */

using UnityEngine;
using UnityEditor;
using System.Collections;

public class ChangeChildTags : MonoBehaviour
{

    [MenuItem("GameObject/Change Children to Parent Tag")]
    public static void ChangeChildrenTags()
    {
        GameObject currentObject = Selection.activeGameObject;
        string parentTag = currentObject.tag;

        if (currentObject != null && currentObject.transform.childCount > 0)
        {
            if (EditorUtility.DisplayDialog("Change child tags to parent tag", "Do you really want to change every child tag to " + parentTag + "?", "Change tags", "Cancel"))
            {
                Transform[] transforms = Selection.GetTransforms(SelectionMode.Deep | SelectionMode.Editable);
                float numberOfTransforms = transforms.Length;
                float counter = 0.0f;
                foreach (Transform childTransform in transforms)
                {
                    counter++;
                    EditorUtility.DisplayProgressBar("Changing tags", "Changing all child object tags to " + parentTag +
                        
                        "(" + (int)counter + "/" +(int)numberOfTransforms + ")", counter / numberOfTransforms);
                    childTransform.gameObject.tag = parentTag;
                }
                EditorUtility.ClearProgressBar();
            }
        }
    }
}