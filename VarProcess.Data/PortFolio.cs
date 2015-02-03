using System.Collections.Generic;

namespace VarProcess.Data
{
    public sealed class Portfolio
    {
        public string Name { get; set; }
        public IEnumerable<Transaction> Transactions { get; set;}
    }
}
