using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fargs.Web.Models
{
    public class Portfolio
    {
        
        private List<Project> mProjects;

        public List<Project> Projects
        {
            get
            {
                if (mProjects == null)
                {
                    mProjects = new List<Project>();
                }
                return mProjects;
            }
        }

        private List<Software> mSoftware;

        public List<Software> Software
        {
            get
            {
                if (mSoftware == null)
                {
                    mSoftware = new List<Software>();
                }
                return mSoftware;
            }
        }
    }
}