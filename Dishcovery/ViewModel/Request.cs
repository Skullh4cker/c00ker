using Dishcovery.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dishcovery.ViewModel
{
    public class Request
    {
        public ObservableCollection<IngredientView> AvailableIngredients { get; set; }
        public ObservableCollection<IngredientView> ProhibitedIngredients { get; set; }
        public ObservableCollection<Tag> DesiredTags { get; set; }
        public ObservableCollection<Tag> ProhibitedTags { get; set; }
        public Request() 
        {
            AvailableIngredients = new ObservableCollection<IngredientView>();
            ProhibitedIngredients = new ObservableCollection<IngredientView>();
            DesiredTags = new ObservableCollection<Tag>();
            ProhibitedTags = new ObservableCollection<Tag>();
        }
    }       
}
