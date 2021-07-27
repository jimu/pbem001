using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Bopper.View
{
    static public class ViewMaster
    {
        static public Action resetListeners;
        static public Action<Bopper.Unit, string> deployListeners;
        static public Action<Bopper.Unit> undeployListeners;
        static public Action<Bopper.Unit, int, string> moveListeners;
        static public Action<string> phaseListeners;
    }
}


namespace Bopper.View.Unity
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] HexGrid hexGrid;
        [SerializeField] TextMeshProUGUI statusMessage;

        Dictionary<long, Unit> idToUnit = new Dictionary<long, Unit>();
        Animation currentAnimation;
        Animator currentAnimator;
        string currentParameter;

        // Start is called before the first frame update
        void Start()
        {
            // Listen for deployment commands
            // add myself to view listener for deploy
            ViewMaster.resetListeners += ResetView;
            ViewMaster.deployListeners += DisplayDeploy;        // this is too late.  Commands that exist will try use.  Listener should be added to a Singleton that knows about all views
            ViewMaster.moveListeners += DisplayMove;
            ViewMaster.undeployListeners += DisplayUndeploy;        // this is too late.  Commands that exist will try use.  Listener should be added to a Singleton that knows about all views
            ViewMaster.phaseListeners += DisplayPhase;

        }

        // Update is called once per frame
        void Update()
        {

        }

        /*
        /// <summary>
        /// Add all Commands handled by this view
        /// </summary>
        /// <param name="command"></param>
        public void Display(Command command)
        {
            System.Type type = command.GetType();

            if (type == typeof(CommandDeploy)) Display((CommandDeploy)command);
            else
                DisplayNone();
        }

        void Display(CommandDeploy command)
        {
            DisplayDeploy(command.unit, command.coord);
        }
        */

        void DisplayDeploy(Bopper.Unit unit, string message)
        {
            Debug.Log($"GameView.DisplayDeploy({unit.name}, {message})");
            HexCoordinates hcoord = HexCoordinates.FromRivets(unit.coord);
            Unit counter = hexGrid.CreateUnitInCell(hcoord, unit.id, unit.data, unit.player_id == 1, unit.layer);

            idToUnit[unit.id] = counter;

            counter.GetComponent<Animator>().SetBool(0, true);
            StartAnimator(counter.GetComponent<Animator>(), "Flashing");
            SetStatusMessage(message);
        }



        void DisplayUndeploy(Bopper.Unit unit)
        {
            Debug.Log($"View.UndeployUnit({unit.id})"); //todo
            HexCoordinates hcoord = HexCoordinates.FromRivets(unit.coord);

            //idToUnit[unit.id] = counter;
            hexGrid.RemoveUnitFromCell(hcoord, unit.id);

            //SetAnimation(counter.GetComponent<Animation>());
            //SetStatusMessage(message);
        }


        void DisplayMove(Bopper.Unit unit, int coord, string message)
        {
            HexCoordinates hcoord = HexCoordinates.FromRivets(unit.coord);

            Unit counter = idToUnit[unit.id];

            SetStatusMessage(message);
        }

        void DisplayPhase(string message)
        {
            StopAnimator();
            SetStatusMessage(message);
        }

        void DisplayNone()
        {
            StopAnimator();
            SetStatusMessage();
        }


        void StartAnimator(Animator animator, string parameter)
        {
            StopAnimator();
            currentAnimator = animator;
            currentParameter = parameter;
            animator.SetBool(parameter, true);
        }

        private void ResetView()
        {
            StopAnimator();
        }

        private void StopAnimator()
        {
            if (currentAnimator)
                currentAnimator.SetBool(currentParameter, false);
            currentAnimator = null;
        }

        void SetStatusMessage(string message = "")
        {
            if (statusMessage)
                statusMessage.text = message;
        }
    }
}