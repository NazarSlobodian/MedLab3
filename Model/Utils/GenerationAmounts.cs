using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLab.Model.Utils
{
    public struct GenerationAmounts
    {
        public int collectionPointAmount;
        public int receptionistsAmount;
        public int patientAmount;
        public int batchesPerPatient;
        public int ordersPerBatch;
        public GenerationAmounts(int collectionPointAmount, int receptionistsAmount, int patientAmount,
            int batchesPerPatient, int ordersPerBatch)
        {
            this.collectionPointAmount = collectionPointAmount;
            this.receptionistsAmount = receptionistsAmount;
            this.patientAmount = patientAmount;
            this.batchesPerPatient = batchesPerPatient;
            this.ordersPerBatch = ordersPerBatch;
        }
    }
}
