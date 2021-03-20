using System;
using System.Collections.Generic;

namespace CalculatorServices.WebAPI.DTOs
{
    public class QueryResponse
    {
        public List<OperationResponse> Operations { get; set; }
    }

    public class OperationResponse
    {
        public string OperationType { get; set; } = "";
        public string Calculation { get; set; } = "";
        public DateTime DateTime { get; set; }
    }
}
