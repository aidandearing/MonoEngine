using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoEngine.Input
{
    class InputProfile
    {
        public enum InputType { Keyboard, KeyboardAndMouse, Controller0, Controller1, Controller2, Controller3, Controller4, Controller5, Controller6, Controller7 };
        public InputType type;
        public string name;

        public MouseState lastMouseState;
        public MouseState currentMouseState;
        public KeyboardState lastKeyboardState;
        public KeyboardState currentKeyboardState;
        public GamePadState lastGamePadState;
        public GamePadState currentGamePadState;

        public InputProfile(string name, InputType type)
        {
            this.type = type;
            this.name = name;
        }

        public void Update()
        {
            switch (type)
            {
                case InputType.Keyboard:
                    lastKeyboardState = currentKeyboardState;
                    currentKeyboardState = Keyboard.GetState();
                    break;
                case InputType.KeyboardAndMouse:
                    lastKeyboardState = currentKeyboardState;
                    currentKeyboardState = Keyboard.GetState();
                    lastMouseState = currentMouseState;
                    currentMouseState = Mouse.GetState();
                    break;
                case InputType.Controller0:
                    lastGamePadState = currentGamePadState;
                    currentGamePadState = GamePad.GetState(0);
                    break;
                case InputType.Controller1:
                    lastGamePadState = currentGamePadState;
                    currentGamePadState = GamePad.GetState(1);
                    break;
                case InputType.Controller2:
                    lastGamePadState = currentGamePadState;
                    currentGamePadState = GamePad.GetState(2);
                    break;
                case InputType.Controller3:
                    lastGamePadState = currentGamePadState;
                    currentGamePadState = GamePad.GetState(3);
                    break;
                case InputType.Controller4:
                    lastGamePadState = currentGamePadState;
                    currentGamePadState = GamePad.GetState(4);
                    break;
                case InputType.Controller5:
                    lastGamePadState = currentGamePadState;
                    currentGamePadState = GamePad.GetState(5);
                    break;
                case InputType.Controller6:
                    lastGamePadState = currentGamePadState;
                    currentGamePadState = GamePad.GetState(6);
                    break;
                case InputType.Controller7:
                    lastGamePadState = currentGamePadState;
                    currentGamePadState = GamePad.GetState(7);
                    break;     
            }
        }

    }
}
