using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class GameFrame
{
    private readonly ReadOnlyCollection<ISpaceObjectKey> items;

    public GameFrame(IEnumerable<ISpaceObjectKey> items)
    {
        this.items = items.ToList().AsReadOnly();
    }

    public void Display(float timeWithinFrame)
    {
        foreach (ISpaceObjectKey item in items)
        {
            item.Display(timeWithinFrame);
        }
    }
}
