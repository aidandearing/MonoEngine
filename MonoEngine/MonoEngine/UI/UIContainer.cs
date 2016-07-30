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
                            this.BoundsAlign = align;
                            this.Bounds = ReaderToBounds(this, reader);
                            break;
                        case "behaviours":
                            this.Behaviours = ReaderToBehaviours(this, reader);
                            break;
                        case "objects":
                            this.ComponentRefs = ReaderToObjectRefs(this, reader);
                            break;
                    }
                }
            }
        }
    }
}
