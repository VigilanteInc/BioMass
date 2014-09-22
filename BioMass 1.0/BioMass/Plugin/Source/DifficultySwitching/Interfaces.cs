using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPBioMass
{
    interface ISavable
    {
        void Load(ConfigNode globalNode);
        void Save(ConfigNode globalNode);
    }
}
