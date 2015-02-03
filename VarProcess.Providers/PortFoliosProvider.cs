// Copyright 2015 ACENSI http://www.acensi.fr/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using VarProcess.Data;

namespace VarProcess.Providers
{
    public class PortfoliosProvider : IPortfoliosProvider
    {
        public virtual IEnumerable<Portfolio> Portfolios { private set; get; }

        public PortfoliosProvider(string path)
        {
            var tempPortfolios = new Dictionary<string,Portfolio>();
            var products = new Dictionary<string,Product>();
            //recuperer all xml files
            var dirInfo = new DirectoryInfo(path);
            var files = dirInfo.GetFiles("*.xml");
            foreach(var file in files)
            {
                //file parsing
                var xmlPortfolio = new XElement("portfolio");
                var xmlname = new XElement("name");
                var xmlTransactions = new XElement("transactions");
                var xmlTransaction = new XElement("transaction");
                var xmlProduct = new XElement("product");
                var xmlPosition = new XElement("position");
                xmlPortfolio = XElement.Load(file.FullName);
                var portName = xmlPortfolio.Element(xmlname.Name).Value;
                var transactions = xmlPortfolio.Element(xmlTransactions.Name).Elements(xmlTransaction.Name);
                if (!tempPortfolios.ContainsKey(portName))
                {
                    tempPortfolios.Add(portName, new Portfolio(){Name=portName});
                    tempPortfolios[portName].Transactions = new List<Transaction>();
                }

                foreach(var trans in transactions)
                {
                    var transaction = new Transaction();
                    transaction.Position = int.Parse(trans.Attribute(xmlPosition.Name).Value);
                    var productName = trans.Attribute(xmlProduct.Name).Value;
                    if (!products.ContainsKey(productName))
                        products.Add(productName, new Product() { Name = productName });
                    transaction.Product = products[productName];
                    (tempPortfolios[portName].Transactions as List<Transaction>).Add(transaction);
                }
                
            }
            Portfolios = tempPortfolios.Values;
        }

    }
}
