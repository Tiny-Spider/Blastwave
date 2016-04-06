using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class InputManager {
    public static float GetAxis(InputDevice device, InputAxis axis) {
        return axis.GetValue(device);
    }

    public static bool GetButton(InputDevice device, InputButton button, params InputButton[] inputButtons) {
        foreach (InputButton inputButton in inputButtons) {
            if (inputButton.GetButton(device)) return true;
        }

        return button.GetButton(device);
    }

    public static bool GetButtonDown(InputDevice device, InputButton button, params InputButton[] inputButtons) {
        foreach (InputButton inputButton in inputButtons) {
            if (inputButton.GetButtonDown(device)) return true;
        }

        return button.GetButtonDown(device);
    }

    public static bool GetButtonUp(InputDevice device, InputButton button, params InputButton[] inputButtons) {
        foreach (InputButton inputButton in inputButtons) {
            if (inputButton.GetButtonUp(device)) return true;
        }

        return button.GetButtonUp(device);
    }
}

public class InputAxis {
    public static readonly InputAxis Horizontal = new InputAxis("horizontal");
    public static readonly InputAxis Vertical = new InputAxis("vertical");
    public static readonly InputAxis HorizontalLook = new InputAxis("horizontalLook");
    public static readonly InputAxis VerticalLook = new InputAxis("verticalLook");
    public static readonly InputAxis Trigger = new InputAxis("trigger");

    private string axis;

    private InputAxis(string axis) {
        this.axis = axis;
    }

    public float GetValue(InputDevice inputDevice) {
        return Input.GetAxisRaw(inputDevice.ToInputName() + axis);
    }
}

public class InputButton {
    public static readonly InputButton Accept = new InputButton("return", "button 0");
    public static readonly InputButton Deny = new InputButton("escape", "button 1");
    public static readonly InputButton Left = new InputButton("a", "button 4");
    public static readonly InputButton Right = new InputButton("d", "button 5");
    public static readonly InputButton Back = new InputButton("escape", "button 6");
    public static readonly InputButton Start = new InputButton("space", "button 7");
    public static readonly InputButton TriggerLeft = new InputButton("mouse 0", "trigger", false);
    public static readonly InputButton TriggerRight = new InputButton("mouse 0", "trigger", true);
    public static readonly InputButton ChooseLeft = new InputButton("q", "trigger", false);
    public static readonly InputButton ChooseRight = new InputButton("e", "trigger", true);

    private bool joystickIsAxis;

    private string keyboardInputName;
    private string joystickInputName;

    private bool invert;
    private HashSet<InputDevice> isAxisTriggered = new HashSet<InputDevice>();

    private InputButton(string keyboardInputName, string joystickInputName) {
        joystickIsAxis = false;

        this.keyboardInputName = keyboardInputName;
        this.joystickInputName = joystickInputName;
    }

    private InputButton(string keyboardInputName, string joystickInputName, bool invert) {
        joystickIsAxis = true;

        this.keyboardInputName = keyboardInputName;
        this.joystickInputName = joystickInputName;
        this.invert = invert;
    }

    public bool GetButton(InputDevice inputDevice) {
        if (inputDevice.IsKeyboard()) {
            return Input.GetKey(keyboardInputName);
        }
        else if (!joystickIsAxis) {
            return Input.GetKey(inputDevice.ToInputName() + joystickInputName);
        }

        return invert ? Input.GetAxisRaw(inputDevice.ToInputName() + joystickInputName) < 0 : Input.GetAxisRaw(inputDevice.ToInputName() + joystickInputName) > 0;
    }

    public bool GetButtonDown(InputDevice inputDevice) {
        if (inputDevice.IsKeyboard()) {
            return Input.GetKeyDown(keyboardInputName);
        }
        else if (!joystickIsAxis) {
            return Input.GetKeyDown(inputDevice.ToInputName() + joystickInputName);
        }

        float input = invert ? Input.GetAxisRaw(inputDevice.ToInputName() + joystickInputName) : -Input.GetAxisRaw(inputDevice.ToInputName() + joystickInputName);

        if (!isAxisTriggered.Contains(inputDevice)) {
            if (input > 0) {
                isAxisTriggered.Add(inputDevice);
                return true;
            }
        }
        else {
            if (input <= 0) {
                isAxisTriggered.Remove(inputDevice);
            }
        }

        return false;
    }

    public bool GetButtonUp(InputDevice inputDevice) {
        if (inputDevice.IsKeyboard()) {
            return Input.GetKeyUp(keyboardInputName);
        }
        else if (!joystickIsAxis) {
            return Input.GetKeyUp(inputDevice.ToInputName() + joystickInputName);
        }

        float input = invert ? Input.GetAxisRaw(inputDevice.ToInputName() + joystickInputName) : -Input.GetAxisRaw(inputDevice.ToInputName() + joystickInputName);

        if (isAxisTriggered.Contains(inputDevice)) {
            if (input <= 0) {
                isAxisTriggered.Remove(inputDevice);
                return true;
            }
        }
        else {
            if (input > 0) {
                isAxisTriggered.Add(inputDevice);
            }
        }

        return false;
    }
}

public enum InputDevice {
    Keyboard,
    AnyController,
    Controller1,
    Controller2,
    Controller3,
    Controller4
}

public static class InputDeviceExtensions {
    public static string ToInputName(this InputDevice inputDevice) {
        switch (inputDevice) {
            case InputDevice.Keyboard:
                return "keyboard ";
            case InputDevice.Controller1:
                return "joystick 1 ";
            case InputDevice.Controller2:
                return "joystick 2 ";
            case InputDevice.Controller3:
                return "joystick 4 ";
            case InputDevice.Controller4:
                return "joystick 5 ";
            default:
                return "joystick ";
        }
    }

    public static bool IsKeyboard(this InputDevice inputDevice) {
        switch (inputDevice) {
            case InputDevice.Keyboard:
                return true;
            default:
                return false;
        }
    }
}