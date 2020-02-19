using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cnrl.Models
{
    public class PayPerTicCrearPago
    {

        public string currency_id { get; set; }
        public string external_transaction_id { get; set; }
        public string due_date { get; set; }
        public string last_due_date { get; set; }
        public List<PayPerTicPagoDetalle> details { get; set; }
        public PayPerTicPagador payer { get; set; }

        public PayPerTicCrearPago()
        {
            details = new List<PayPerTicPagoDetalle>();
        }
    }
}