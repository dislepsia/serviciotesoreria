namespace cnrl.Models
{
    public class PayPerTicPagoDetalle
    {
        public string external_reference { get; set; }
        public string concept_id { get; set; }
        public string concept_description { get; set; }
        public decimal amount { get; set; }

    }
}