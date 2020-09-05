using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatGeneralLoader : MonoBehaviour
{

    public const string path = "featsgeneral.xml";

    // Start is called before the first frame update
    void Start()
    {
         FeatGeneralContainer fgc = FeatGeneralContainer.Load(path);

        foreach(FeatGeneral feat in fgc.feats)
        {
            print(feat.featname);
        }
    }

}
