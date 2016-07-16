using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MonoEngine.UI
{
    class UIContainer : UIObject
    {
        public UIContainer(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "bounds":
                            Alignment align = new Alignment();
                            Enum.TryParse<Alignment>(reader["origin"], out align);
                            BoundsAlign = align;
                            Bounds = ReaderToBounds(this, reader);
                            break;
                        case "behaviours":
                            Behaviours = ReaderToBehaviours(this, reader);
                            break;
                        case "objects":
                            ComponentRefs = ReaderToObjectRefs(this, reader);
                            break;
                    }
                }
            }
        }
    }
}
