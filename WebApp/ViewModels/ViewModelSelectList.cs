using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.ViewModels
{
    public class ViewModelSelectList<T, ID>
    {
        public List<T> Items { get; set; }
        public ID SelectedItemId { get; set; }
    }
}