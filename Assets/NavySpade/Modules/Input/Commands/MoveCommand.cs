using Core.Input.Commands.Interfaces;
using UnityEngine;

namespace Core.Input.Commands
{
    public class MoveCommand : ICommand
    {
        private Vector3 _movement;

        public MoveCommand(Vector3 movement)
        {
            _movement = movement;
        }

        public void Execute()
        {

        }
    }
}