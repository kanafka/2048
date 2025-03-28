using NUnit.Framework;
using UnityEngine;

namespace New_Folder
{
    public class InputManagerTests
    {
        private InputManager inputManager;
        private GameField gameField;

        [SetUp]
        public void SetUp()
        {
            GameObject inputObj = new GameObject("InputManager");
            inputManager = inputObj.AddComponent<InputManager>();

            GameObject gameFieldObj = new GameObject("GameField");
            gameField = gameFieldObj.AddComponent<GameField>();
        }
    }

}