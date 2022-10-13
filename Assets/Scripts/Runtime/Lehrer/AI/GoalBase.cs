using System;
using System.Collections;
using System.Collections.Generic;

namespace HerderGames.Lehrer.AI
{
    public abstract class GoalBase
    {
        public Lehrer Lehrer { get; private set; }

        public void InitLehrer(Lehrer lehrer)
        {
            if (Lehrer != null)
            {
                throw new Exception($"{nameof(InitLehrer)} called twice");
            }
            
            Lehrer = lehrer;
        }
        
        /**
         * Wird ausgeführt, wenn die Ausführung dieses Goals in Frage kommt.
         *
         * Wenn das Goal allerdings nicht starten kann, muss der zurückgegebene Enumerator sofort beendet werden.
         * Wenn das Goal ausgeführt werden kann, muss als erstes null zurückgegeben werden.
         * 
         * Wenn das Goal gestartet werden konnte, wird in jedem Frame der Enumerator um einen Schritt bewegt.
         * Wenn der Enumerator benndet, wird das Goal nicht weiter ausgeführt, aber unexpectedGoalEndCallback wird nicht ausgeführt.
         *
         * Wenn es ein Goal mit einer höheren Priorität gibt, das startetn kann wird unexpectedGoalEndCallback ausgeführt und das Goal beendet.
         */
        public abstract IEnumerable ExecuteGoal(Stack<Action> unexpectedGoalEndCallback);

        /**
         * Wird genau einmal ausgeführt, wenn das Goal einem Lehrer zugewiesen wird. Der zurückgegebene Enumerator wird solange in jedem Frame
         * einen Schritt weiter bewegt, bis dieser endet.
         */
        public virtual IEnumerable UpdateGoal()
        {
            yield break;
        }
    }
}