using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLab.Model.Utils
{
    public struct GenerationAmounts
    {
        public int labAmount;
        public int techAmount;
        public int patientAmount;
        public int batchesPerPatient;
        public int ordersPerBatch;
        public GenerationAmounts(int labAmount, int techAmount, int patientAmount,
            int batchesPerPatient, int ordersPerBatch)
        {
            this.labAmount = labAmount;
            this.techAmount = techAmount;
            this.patientAmount = patientAmount;
            this.batchesPerPatient = batchesPerPatient;
            this.ordersPerBatch = ordersPerBatch;
        }
    }
}
