using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.ViewModels.UIElements
{
    public class DropDownListViewModel<T>
    {
        public string Name { get; set; }
        public bool TriggerFormSubmit { get; set; }
        public T SelectedValue { get; set; }
        public IEnumerable<SelectListItem> SelectList { get; set; }
    }
}