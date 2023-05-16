using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dishcovery;

public class RequestFilter
{
    public List<IngredientView> RequiredIngredients { get; private set; }
    public List<IngredientView> IgnoredIngredients { get; private set; }
    public List<Tag> WantedTags { get; private set; }
    public List<Tag> UnwantedTags { get; private set; }

    public RequestFilter(
        List<IngredientView> required,
        List<IngredientView> ignored,
        List<Tag> wantedTags,
        List<Tag> unwantedTags)
    {
        RequiredIngredients = required;
        IgnoredIngredients = ignored;
        WantedTags = wantedTags;
        UnwantedTags = unwantedTags;
    }

    public RequestFilter()
    {
        RequiredIngredients = new List<IngredientView>();
        IgnoredIngredients = new List<IngredientView>();
        WantedTags = new List<Tag>();
        UnwantedTags = new List<Tag>();
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
}
