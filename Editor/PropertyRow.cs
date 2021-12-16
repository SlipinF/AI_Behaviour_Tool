using System;
using System.Linq;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace LoD.BT.Editor {
  public class PropertyRow : VisualElement {

    public VisualElement component;
    public Port port;

    public PropertyRow(string name, VisualElement component, Port port = null) {
      this.component = component;
      this.port = port;

      VisualElement container = new VisualElement { name = "container" };

      if(port != null) {
        VisualElement portContainer = new VisualElement { name = "port" };
        portContainer.Add(port);
        container.Add(portContainer);
      } else {
        VisualElement labelContainer = new VisualElement { name = "label" };
        labelContainer.Add(new Label(name));
        container.Add(labelContainer);
      }

      VisualElement contentContainer = new VisualElement { name = "content" };
      contentContainer.Add(component);

      container.Add(contentContainer);

      Add(container);
    }
  }
}
