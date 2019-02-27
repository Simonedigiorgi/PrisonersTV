using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum AxisType
{
    KeyOrMouseButton = 0,
    MouseMovement = 1,
    JoystickAxis = 2
};

public class InputAxis
{
    public string name;
    public string descriptiveName;
    public string descriptiveNegativeName;
    public string negativeButton;
    public string positiveButton;
    public string altNegativeButton;
    public string altPositiveButton;

    public float gravity;
    public float dead;
    public float sensitivity;

    public bool snap = false;
    public bool invert = false;

    public AxisType type;

    public int axis;
    public int joyNum;
}

public class EditInputManager : MonoBehaviour
{
    private static int indexCount;
    private static SerializedProperty GetChildProperty(SerializedProperty parent, string name)
    {
        SerializedProperty child = parent.Copy();
        child.Next(true);
        do
        {
            if (child.name == name) return child;
        }
        while (child.Next(false));
        return null;
    }

    private static bool AxisDefined(string axisName)
    {
        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

        axesProperty.Next(true);
        axesProperty.Next(true);
        while (axesProperty.Next(false))
        {
            SerializedProperty axis = axesProperty.Copy();
            axis.Next(true);
            if (axis.stringValue == axisName) return true;
        }
        return false;
    }

    private static bool AxisIndex(string axisName)
    {
        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");
        indexCount = -1;
        axesProperty.Next(true);
        axesProperty.Next(true);
        while (axesProperty.Next(false))
        {
            indexCount++;
            SerializedProperty axis = axesProperty.Copy();
            axis.Next(true);
            if (axis.stringValue == axisName) return true;
        }
        return false;
    }

    private static void AddAxis(InputAxis axis)
    {
        if (AxisDefined(axis.name)) return;

        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

        axesProperty.arraySize++;
        serializedObject.ApplyModifiedProperties();

        SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);

        GetChildProperty(axisProperty, "m_Name").stringValue = axis.name;
        GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
        GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
        GetChildProperty(axisProperty, "negativeButton").stringValue = axis.negativeButton;
        GetChildProperty(axisProperty, "positiveButton").stringValue = axis.positiveButton;
        GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
        GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
        GetChildProperty(axisProperty, "gravity").floatValue = axis.gravity;
        GetChildProperty(axisProperty, "dead").floatValue = axis.dead;
        GetChildProperty(axisProperty, "sensitivity").floatValue = axis.sensitivity;
        GetChildProperty(axisProperty, "snap").boolValue = axis.snap;
        GetChildProperty(axisProperty, "invert").boolValue = axis.invert;
        GetChildProperty(axisProperty, "type").intValue = (int)axis.type;
        GetChildProperty(axisProperty, "axis").intValue = axis.axis - 1;
        GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;

        serializedObject.ApplyModifiedProperties();
    }

    private static void EditAxisOwner(int controllerIndex, string axisName, int oldIndex)
    {       
        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");
        SerializedProperty axisProperty;

        if (AxisDefined(axisName))
        {
            axisProperty = axesProperty.GetArrayElementAtIndex(indexCount);
            GetChildProperty(axisProperty, "positiveButton").stringValue.Replace("joystick "+ oldIndex, "joystick " + controllerIndex);
        }
        serializedObject.ApplyModifiedProperties();
    }

    private static void JoystickConfigSetup(int joyNumb)
    {
        AddAxis(new InputAxis()
        {
            name = "J" + joyNumb + "_Horizontal",
            gravity = 0,
            dead = 0.2f,
            sensitivity = 1f,
            snap = false,
            invert = false,
            type = AxisType.JoystickAxis,
            axis = 1,
            joyNum = joyNumb,

        });
        AddAxis(new InputAxis()
        {
            name = "J" + joyNumb + "_Vertical",
            gravity = 0,
            dead = 0.2f,
            sensitivity = 1f,
            snap = false,
            invert = true,
            type = AxisType.JoystickAxis,
            axis = 2,
            joyNum = joyNumb,

        });
        AddAxis(new InputAxis()
        {
            name = "J" + joyNumb + "_Horizontal_R",
            gravity = 0,
            dead = 0.2f,
            sensitivity = 1f,
            snap = false,
            invert = false,
            type = AxisType.JoystickAxis,
            axis = 4,
            joyNum = joyNumb,

        });
        AddAxis(new InputAxis()
        {
            name = "J" + joyNumb + "_Vertical_R",
            gravity = 0,
            dead = 0.2f,
            sensitivity = 1f,
            snap = false,
            invert = true,
            type = AxisType.JoystickAxis,
            axis = 5,
            joyNum = joyNumb,

        });
        AddAxis(new InputAxis()
        {
            name = "J" + joyNumb + "_Button_A",
            positiveButton = "joystick " + joyNumb + " button 0",
            gravity = 1000,
            dead = 0.001f,
            sensitivity = 1000f,
            snap = false,
            invert = false,
            type = AxisType.KeyOrMouseButton,
            axis = 1,
            joyNum = joyNumb,

        });
        AddAxis(new InputAxis()
        {
            name = "J" + joyNumb + "_Button_B",
            positiveButton = "joystick " + joyNumb + " button 1",
            gravity = 1000,
            dead = 0.001f,
            sensitivity = 1000f,
            snap = false,
            invert = false,
            type = AxisType.KeyOrMouseButton,
            axis = 1,
            joyNum = joyNumb,

        });
        AddAxis(new InputAxis()
        {
            name = "J" + joyNumb + "_Button_X",
            positiveButton = "joystick " + joyNumb + " button 2",
            gravity = 1000,
            dead = 0.001f,
            sensitivity = 1000f,
            snap = false,
            invert = false,
            type = AxisType.KeyOrMouseButton,
            axis = 1,
            joyNum = joyNumb,

        });
        AddAxis(new InputAxis()
        {
            name = "J" + joyNumb + "_Button_Y",
            positiveButton = "joystick " + joyNumb + " button 3",
            gravity = 1000,
            dead = 0.001f,
            sensitivity = 1000f,
            snap = false,
            invert = false,
            type = AxisType.KeyOrMouseButton,
            axis = 1,
            joyNum = joyNumb,

        });
        AddAxis(new InputAxis()
        {
            name = "J" + joyNumb + "_Start",
            positiveButton = "joystick " + joyNumb + " button 7",
            gravity = 1000,
            dead = 0.001f,
            sensitivity = 1000f,
            snap = false,
            invert = false,
            type = AxisType.KeyOrMouseButton,
            axis = 1,
            joyNum = joyNumb,

        });
        AddAxis(new InputAxis()
        {
            name = "J" + joyNumb + "_Pause",
            positiveButton = "joystick " + joyNumb + " button 6",
            gravity = 1000,
            dead = 0.001f,
            sensitivity = 1000f,
            snap = false,
            invert = false,
            type = AxisType.KeyOrMouseButton,
            axis = 1,
            joyNum = joyNumb,

        });
        AddAxis(new InputAxis()
        {
            name = "J" + joyNumb + "_RB",
            positiveButton = "joystick " + joyNumb + " button 5",
            gravity = 1000,
            dead = 0.001f,
            sensitivity = 1000f,
            snap = false,
            invert = false,
            type = AxisType.KeyOrMouseButton,
            axis = 1,
            joyNum = joyNumb,

        });
        AddAxis(new InputAxis()
        {
            name = "J" + joyNumb + "_LB",
            positiveButton = "joystick " + joyNumb + " button 4",
            gravity = 1000,
            dead = 0.001f,
            sensitivity = 1000f,
            snap = false,
            invert = false,
            type = AxisType.KeyOrMouseButton,
            axis = 1,
            joyNum = joyNumb,

        });
    }
    private static void KeyboardConfigSetup()
    {
        //Add mouse definitions
        AddAxis(new InputAxis() { name = "KM_Horizontal", sensitivity = 0.1f, type = AxisType.MouseMovement, axis = 1 });
        AddAxis(new InputAxis() { name = "KM_Vertical", sensitivity = 0.1f, type = AxisType.MouseMovement, axis = 2 });
        AddAxis(new InputAxis()
        {
            name = "KM_LeftRight",
            negativeButton = "a",
            positiveButton = "d",
            gravity = 1000,
            dead = 0.001f,
            sensitivity = 1000f,
            type = AxisType.KeyOrMouseButton,
            axis = 1
        });
        AddAxis(new InputAxis()
        {
            name = "KM_UpDown",
            negativeButton = "s",
            positiveButton = "w",
            gravity = 1000,
            dead = 0.001f,
            sensitivity = 1000f,
            type = AxisType.KeyOrMouseButton,
            axis = 1
        });
        AddAxis(new InputAxis()
        {
            name = "KM_Jump",
            positiveButton = "space",
            gravity = 1000, dead = 0.001f ,
            sensitivity = 1000f,
            type = AxisType.KeyOrMouseButton,
            axis = 1
        });
        AddAxis(new InputAxis()
        {
            name = "KM_Headbutt",
            positiveButton = "mouse 1",
            gravity = 1000,
            dead = 0.001f,
            sensitivity = 1000f,
            type = AxisType.KeyOrMouseButton,
            axis = 1
        });
        AddAxis(new InputAxis()
        {
            name = "KM_Shoot",
            positiveButton = "mouse 0",
            gravity = 1000,
            dead = 0.001f,
            sensitivity = 1000f,
            type = AxisType.KeyOrMouseButton,
            axis = 1
        });    
        AddAxis(new InputAxis()
        {
            name = "KM_Dodge",
            positiveButton = "left shift",
            gravity = 1000,
            dead = 0.001f,
            sensitivity = 1000f,
            type = AxisType.KeyOrMouseButton,
            axis = 1
        });
        AddAxis(new InputAxis()
        {
            name = "KM_Pause",
            positiveButton = "escape",
            gravity = 1000,
            dead = 0.001f,
            sensitivity = 1000f,
            type = AxisType.KeyOrMouseButton,
            axis = 1
        });
        AddAxis(new InputAxis()
        {
            name = "KM_Start",
            positiveButton = "enter",
            gravity = 1000,
            dead = 0.001f,
            sensitivity = 1000f,
            type = AxisType.KeyOrMouseButton,
            axis = 1
        });
        AddAxis(new InputAxis()
        {
            name = "KM_Interact",
            positiveButton = "f",
            gravity = 1000,
            dead = 0.001f,
            sensitivity = 1000f,
            type = AxisType.KeyOrMouseButton,
            axis = 1
        });
        AddAxis(new InputAxis()
        {
            name = "KM_RB",
            positiveButton = "e",
            gravity = 1000,
            dead = 0.001f,
            sensitivity = 1000f,
            type = AxisType.KeyOrMouseButton,
            axis = 1
        });
        AddAxis(new InputAxis()
        {
            name = "KM_LB",
            positiveButton = "q",
            gravity = 1000,
            dead = 0.001f,
            sensitivity = 1000f,
            type = AxisType.KeyOrMouseButton,
            axis = 1
        });
    }

    [MenuItem("Custom Input/Setup Input Manager")]
    public static void SetupInputManager()
    {
        KeyboardConfigSetup();

        //Add gamepad definitions
        //int i = 1;
        for (int i = 1; i <= 10; i++)
        {
            for (int j = 0; j <= 10; j++)
            {
                JoystickConfigSetup(i);
            }
        }
    }
    [MenuItem("Custom Input/Clear Input Manager")]
    public static void ClearInputManager()
    {
        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");
        axesProperty.ClearArray();
        serializedObject.ApplyModifiedProperties();
    }
}
