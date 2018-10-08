using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Work.Views.Additionals
{
    public class IndexViewModel
    {
        public IndexViewModel(AdditionalsViewModel additionals)
        {
            Additionals = additionals;
        }
        public AdditionalsViewModel Additionals { get; private set; }
    }
}