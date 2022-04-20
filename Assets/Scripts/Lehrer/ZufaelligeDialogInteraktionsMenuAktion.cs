using UnityEngine;

namespace HerderGames.Lehrer
{
    public class ZufaelligeDialogInteraktionsMenuAktion : ZufaelligeInteraktionsMenuAktion
    {
        private bool InTriggerCollider;
        
        protected override bool ShouldShowInInteraktionsMenu()
        {
            return InTriggerCollider;
        }

        private void OnTriggerEnter(Collider other)
        {
            InTriggerCollider = true;
        }

        private void OnTriggerExit(Collider other)
        {
            InTriggerCollider = false;
        }
    }
}