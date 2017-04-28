using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class TextScenario : MonoBehaviour
    {
        public ArtifactScreen Screen;
        public int MaxLength = 10;
        public int StartingCivs = 2;

        void Start()
        {
            /*AssetDatabase db = GetComponent<AssetDatabase>();
            AssetDatabase.UniverseSegment universe = new AssetDatabase.UniverseSegment(db, StartingCivs, StartingCivs, MaxLength);
            while (universe.NextEra())
            {
                AssetDatabase.ArtifactOutput ao = universe.CreateArtifact();
                if ( ao != null )
                {
                    print(ao.name + " (date: " + ao.date.ToString() + ") ==> " + ao.data);
                    Screen.AddArtifact(new Artifact(ao.name, "Dated to " + ao.date.ToString() + "\r\n...\r\n" + ao.data));
                }
            }*/
        }
    }
}