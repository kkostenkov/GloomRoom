using UnityEngine;
using System.Collections;

namespace Objects
{
    namespace Interactible
    {
        public interface IInteractible
        {
            void Activate();
        }
        public class Button: MonoBehaviour, IInteractible {
            protected bool isActive;
            public virtual void Activate()
            {
                
            }
        }
    }
}
