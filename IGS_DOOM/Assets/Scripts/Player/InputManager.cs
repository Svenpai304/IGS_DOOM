using UnityEngine;

namespace Player
{
    public class InputManager
    {
        public InputManager()
        {
            GameManager.GlobalUpdate += Update;
        }

        public void BindInput(KeyCode _key, ICommand _command)
        {
        
        }

        private void Update()
        {
            Debug.Log("TestLog");
        }
    }
}
