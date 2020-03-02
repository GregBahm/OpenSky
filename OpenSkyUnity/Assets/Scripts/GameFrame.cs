using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class GameFrame
{
    private readonly ReadOnlyCollection<ISpaceObjectAnimator> items;

    public GameFrame(IEnumerable<ISpaceObjectAnimator> items)
    {
        this.items = items.ToList().AsReadOnly();
    }

    public void Display(float timeWithinFrame)
    {
        foreach (ISpaceObjectAnimator item in items)
        {
            item.Display(timeWithinFrame);
        }
    }
}
