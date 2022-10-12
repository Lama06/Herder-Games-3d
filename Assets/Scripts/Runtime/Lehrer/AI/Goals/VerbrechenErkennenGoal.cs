using System;
using System.Collections;
using System.Collections.Generic;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Animation;
using HerderGames.Lehrer.Sprache;
using HerderGames.Util;

namespace HerderGames.Lehrer.AI.Goals
{
    public class VerbrechenErkennenGoal : GoalBase
    {
        private const string WarningMsg = "Achtung: Ein Lehrer hat gemerkt, dass du ein Verbrechen begehen wolltest. " +
                                          "Pass auf, dass er nicht zur Schulleitung geht und dich meldet. " +
                                          "Versuche deine Beziehung zum Lehrer wieder zu verbessern.";

        private readonly TriggerBase Trigger;
        private readonly Player.Player Player;
        private readonly float SchwereMindestens;
        private readonly ISaetzeMoeglichkeitenEinmalig Reaktion;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeWeg;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeAngekommen;
        private readonly AbstractAnimation AnimationWeg;
        private readonly AbstractAnimation AnimationAngekommen;

        public VerbrechenErkennenGoal(
            TriggerBase trigger,
            Player.Player player,
            float schwereMindestens = 0f,
            ISaetzeMoeglichkeitenEinmalig reaktion = null,
            ISaetzeMoeglichkeitenMehrmals saetzeWeg = null,
            ISaetzeMoeglichkeitenMehrmals saetzeAngekommen = null,
            AbstractAnimation animationWeg = null,
            AbstractAnimation animationAngekommen = null
        )
        {
            Trigger = trigger;
            Player = player;
            SchwereMindestens = schwereMindestens;
            Reaktion = reaktion;
            SaetzeWeg = saetzeWeg;
            SaetzeAngekommen = saetzeAngekommen;
            AnimationWeg = animationWeg;
            AnimationAngekommen = animationAngekommen;
        }

        private bool SiehtVerbrechen => Lehrer.Vision.CanSee(Player.gameObject) &&
                                        Player.VerbrechenManager.BegehtGeradeEinVerbrechen &&
                                        Player.VerbrechenManager.Schwere >= SchwereMindestens;

        public override IEnumerable ExecuteGoal(Stack<Action> unexpectedGoalEndCallback)
        {
            if (!Trigger.ShouldRun || !SiehtVerbrechen)
            {
                yield break;
            }

            yield return null;

            Player.Chat.SendChatMessage(WarningMsg);
            Lehrer.Sprache.Say(Reaktion);
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeWeg;
            Lehrer.AnimationManager.CurrentAnimation = AnimationWeg;
            Lehrer.Reputation.AddReputation(-Player.VerbrechenManager.Schwere);
            Player.VerbrechenManager.VerbrechenAbbrechen();

            foreach (var _ in NavMeshUtil.Pathfind(Lehrer, Player.transform.position))
            {
                if (!Trigger.ShouldRun)
                {
                    yield break;
                }

                yield return null;
            }
            
            Lehrer.AnimationManager.CurrentAnimation = AnimationAngekommen;
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAngekommen;
            
            foreach (var _ in IteratorUtil.WaitForSeconds(5))
            {
                if (!Trigger.ShouldRun)
                {
                    yield break;
                }
                
                yield return null;
            }
        }
    }
}
