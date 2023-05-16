using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dishcovery;

public class RequestFilter
{
    public ObservableCollection<IngredientView> RequiredIngredients { get; private set; }
    public ObservableCollection<IngredientView> IgnoredIngredients { get; private set; }
    public ObservableCollection<Tag> WantedTags { get; private set; }
    public ObservableCollection<Tag> UnwantedTags { get; private set; }

    public RequestFilter(ObservableCollection<IngredientView> required, ObservableCollection<IngredientView> ignored, ObservableCollection<Tag> wantedTags, ObservableCollection<Tag> unwantedTags)
    {
        RequiredIngredients = required;
        IgnoredIngredients = ignored;
        WantedTags = wantedTags;
        UnwantedTags = unwantedTags;
    }

    public RequestFilter()
    {
        RequiredIngredients = new ObservableCollection<IngredientView>();
        IgnoredIngredients = new ObservableCollection<IngredientView>();
        WantedTags = new ObservableCollection<Tag>();
        UnwantedTags = new ObservableCollection<Tag>();
    }

    public void DisableIngredient(IngredientView ingredient)
    {
        int id = ingredient.ID;
        if (!IgnoredIngredients.Any(x => x.ID == id))
        {
            IgnoredIngredients.Add(ingredient);
        }
    }
    public void EnableIngredient(IngredientView ingredient)
    {
        int id = ingredient.ID;
        if (IgnoredIngredients.Any(x => x.ID == id))
        {
            IgnoredIngredients.Remove(ingredient);
        }
    }
    public void UnrequireIngredient(IngredientView ingredient)
    {
        int id = ingredient.ID;
        if (RequiredIngredients.Any(x => x.ID == id))
        {
            RequiredIngredients.Remove(ingredient);
        }
    }
    public void RequireIngredient(IngredientView ingredient)
    {
        int id = ingredient.ID;
        if (!RequiredIngredients.Any(x => x.ID == id))
        {
            RequiredIngredients.Add(ingredient);
        }
    }

    public void RequireTag(Tag tag)
    {
        if (!WantedTags.Contains(tag))
        {
            WantedTags.Add(tag);
        }
    }

    public void UnwantTag(Tag tag)
    {
        if (!UnwantedTags.Contains(tag))
        {
            UnwantedTags.Add(tag);
        }
    }
    public void RemoveWantedTag(Tag tag)
    {
        if (WantedTags.Contains(tag))
        {
            WantedTags.Remove(tag);
        }
    }

    public void RemoveUnwantedTag(Tag tag)
    {
        if (UnwantedTags.Contains(tag))
        {
            UnwantedTags.Remove(tag);
        }
    }
    public void ClearAll()
    {
        RequiredIngredients.Clear();
        IgnoredIngredients.Clear();
        WantedTags.Clear();
        UnwantedTags.Clear();
    }
}
