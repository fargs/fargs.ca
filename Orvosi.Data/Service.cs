using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orvosi.Data
{
    public partial class Service
    {
        public bool HasReportDeliverable()
        {
            return ServiceCategoryId == ServiceCategories.AddOn || ServiceCategoryId == ServiceCategories.IndependentMedicalExam;
        }

        public bool CanBeRescheduled()
        {
            return ServiceCategory.Id == ServiceCategories.IndependentMedicalExam || ServiceCategory.Id == ServiceCategories.MedicalConsultation;
        }

        public bool HasAppointment
        {
            get
            {
                return ServiceCategory.Id == ServiceCategories.IndependentMedicalExam || ServiceCategory.Id == ServiceCategories.MedicalConsultation;
            }
        }
    }
}
