using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMOD;

namespace VocabularyBomb
{
    public class FmodFactory
    {
        #region Our INSTANCE (SINGLETON) 
        private static FmodFactory instance = null;
        public static FmodFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FmodFactory();
                }
                return instance;
            }
        }
        #endregion
        #region Fmod Variables 
        private FMOD.EventGroup     eventgroup      = null;
        private FMOD.EventSystem    eventsystem     = null;
        private FMOD.EventCategory  mastercategory  = null;
        private FMOD.RESULT         result;
        private const String path = "Media/";
        #endregion
        /// <summary>
        /// Init the media path  & eventsystem
        /// </summary>
        #region Constructor(s)
        public FmodFactory() 
        {
            // let's create our Event 
            result = FMOD.Event_Factory.EventSystem_Create(ref eventsystem);
            ERRCHECK(result);

            // Init the event system object
            result = eventsystem.init(256, FMOD.INITFLAGS.NORMAL, (IntPtr)null, FMOD.EVENT_INITFLAGS.NORMAL);
            ERRCHECK(result);

            // Set the FMOD's default media Path
            result = eventsystem.setMediaPath(path);
            ERRCHECK(result);

            // Load a .fev file exported from FMOD Designer
            result = eventsystem.load("source.fev");
            ERRCHECK(result);

            // Acces to the group data embedded in the .fev file
            result = eventsystem.getGroup("source/Noises", false, ref eventgroup);
            ERRCHECK(result);
        }
        #endregion
        #region Methodes
        /// <summary>
        /// Loads a new FMOD sound system.
        /// </summary>
        /// <param name="eventName">
        /// The name of the event to call.
        /// </param>
        /// <param name="paramName">
        /// The name of the param that will be applied on our sound  .
        /// </param>
        public void load(string eventName, string paramName,ref  FMOD.Event evt,ref FMOD.EventParameter evtParam)
        {
            // Acces to the Gem EVENT 
            eventgroup.getEvent(eventName, FMOD.EVENT_MODE.DEFAULT, ref evt);

            // Acces to the master CATEGORY
            eventsystem.getCategory("master", ref mastercategory);

            // We CALL our param to handle 
            evt.getParameter(paramName, ref evtParam);
        }

        public void load(string eventName, ref  FMOD.Event evt)
        {
            // Acces to the Gem EVENT 
            eventgroup.getEvent(eventName, FMOD.EVENT_MODE.DEFAULT, ref evt);

            // Acces to the master CATEGORY
            eventsystem.getCategory("master", ref mastercategory);
        }

        // Can INIT our param
        public void setParamValue(int value,ref FMOD.EventParameter evtParam)
        {
            evtParam.setValue(value);
        }

        // PLAY THE SOUND OF THE CURRENT EVENT
        public void start(ref FMOD.Event  evt) 
        {
            evt.start();
        }

        // Stop's the sound of the current Event
        public void stop(ref FMOD.Event evt) 
        {
            evt.stop();
        }

        // Update The current Event 
        public void update(float value, ref FMOD.EventParameter evtParam) 
        {
            evtParam.setValue(value);
        }

        // Checking the FMOD's result
        void ERRCHECK(FMOD.RESULT result)
        {
            if (result != FMOD.RESULT.OK)
            {
                Console.WriteLine("FMOD error! " + result + " - " + FMOD.Error.String(result));
            }
        }
        #endregion
    }
}
