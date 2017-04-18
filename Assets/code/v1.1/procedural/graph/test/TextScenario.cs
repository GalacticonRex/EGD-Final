using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class TextScenario : MonoBehaviour
    {
        public ArtifactScreen Screen;

        void Start()
        {
            AssetDatabase db = GetComponent<AssetDatabase>();
            AssetDatabase.UniverseSegment universe = new AssetDatabase.UniverseSegment(db, 2, 10, 10);
            while (universe.NextEra())
            {
                AssetDatabase.ArtifactOutput ao = universe.CreateArtifact();
                if ( ao != null )
                {
                    print(ao.name + " (date: " + ao.date.ToString() + ") ==> " + ao.data);
                    Screen.AddArtifact(new Artifact(ao.name, "Dated to " + ao.date.ToString() + "\r\n...\r\n" + ao.data));
                }
            }
        }
    }
}