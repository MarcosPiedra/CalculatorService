using System;

namespace CalculatorService.Domain.Models
{
    public class Operation
    {
        public int Id { get; set; }
        public string OperationType { get; set; }
        public string Calculation { get; set; }
        public DateTime DateTime { get; set; }
        public int TrackId { get; set; }
    }
}
