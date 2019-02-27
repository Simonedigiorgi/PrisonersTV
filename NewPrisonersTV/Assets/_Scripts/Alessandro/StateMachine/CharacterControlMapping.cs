[System.Serializable]
public class CharacterControlMapping
{
    public string LeftHorizontal;                                           
    public string LeftVertical;                                             
    public string RightHorizontal;                                           
    public string RightVertical;                                             
    public string shootInput;                         
    public string jumpInput;                          
    public string interactInput;
    public string respawnInput;
    public string pauseInput;
    public string dodgeInput;
    public string headbuttInput;
    public bool moveArmWithRightStick;

    public CharacterControlMapping(CharacterControlConfig input, int index)
    {
        if (index != GMController.instance.KeyboardConfig.ControllerIndex)
        {
            LeftHorizontal = input.controller.ToString() + (index+1) + input.LeftHorizontal;
            LeftVertical = input.controller.ToString() + (index + 1) + input.LeftVertical;
            RightHorizontal = input.controller.ToString() + (index + 1) + input.RightHorizontal;
            RightVertical = input.controller.ToString() + (index + 1) + input.RightVertical;
            shootInput = input.controller.ToString() + (index + 1) + input.shootInput;
            jumpInput = input.controller.ToString() + (index + 1) + input.jumpInput;
            interactInput = input.controller.ToString() + (index + 1) + input.interactInput;
            respawnInput = input.controller.ToString() + (index + 1) + input.respawnInput;
            pauseInput = input.controller.ToString() + (index + 1) + input.pauseInput;
            dodgeInput = input.controller.ToString() + (index + 1) + input.dodgeInput;
            headbuttInput = input.controller.ToString() + (index + 1) + input.headbuttInput;
        }
        else
        {
            LeftHorizontal = input.controller.ToString() + input.LeftHorizontal;
            LeftVertical = input.controller.ToString() + input.LeftVertical;
            RightHorizontal = input.controller.ToString() + input.RightHorizontal;
            RightVertical = input.controller.ToString() + input.RightVertical;
            shootInput = input.controller.ToString() + input.shootInput;
            jumpInput = input.controller.ToString() + input.jumpInput;
            interactInput = input.controller.ToString() + input.interactInput;
            respawnInput = input.controller.ToString() + input.respawnInput;
            pauseInput = input.controller.ToString() + input.pauseInput;
            dodgeInput = input.controller.ToString() + input.dodgeInput;
            headbuttInput = input.controller.ToString() + input.headbuttInput;
        }
        moveArmWithRightStick = input.moveArmWithRightStick;
    }
}
