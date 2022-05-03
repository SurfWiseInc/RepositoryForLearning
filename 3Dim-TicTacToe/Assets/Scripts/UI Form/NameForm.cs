using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameForm : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_InputField InputField;

    private bool IsSubmitting = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if (InputField == null)
        {
            throw new SystemException("missing input field");
        }    
    }
    
    public void OnSubmitClick()
    {
        if (IsSubmitting) {
            return;
        }
        string name = InputField.text.Trim();

        if (name == string.Empty)
        {
            return;
        }

        if (NetworkingManager.Instance.ConnectionState == DarkRift.ConnectionState.Connected ||
            NetworkingManager.Instance.Connect())
        {
            IsSubmitting = true;
            NetworkingManager.Instance.MessageNameToServer(name);
        }
    }
}
