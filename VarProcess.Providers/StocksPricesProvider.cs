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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using VarProcess.Data;

namespace VarProcess.Providers
{
    public class StocksPricesProvider : IProductParametersProvider
    {
        public virtual IEnumerable<ProductParameters> ProductsParameters { private set; get; }

        public StocksPricesProvider(string path)
        {
            ProductsParameters = new List<ProductParameters>();
            var products = new Dictionary<string, Product>();
            //recuperer all xml files
            var dirInfo = new DirectoryInfo(path);
            var file = dirInfo.GetFiles("*.xml").First();

            //file parsing
            var xmlProduct = new XElement("product");
            var xmlName = new XElement("name");
            var xmlPrice = new XElement("price");
            var xmlMean = new XElement("mean");
            var xmlStandarddeviation = new XElement("standarddeviation");
            var xlmRoot = XElement.Load(file.FullName);
            var xmlProducts = xlmRoot.Elements(xmlProduct.Name);
            foreach (var prod in xmlProducts)
            {
                var productParameters = new ProductParameters();
                var productName = prod.Element(xmlName.Name).Value;
                if (!products.ContainsKey(productName))
                    products.Add(productName, new Product() { Name = productName });
                productParameters.Product = products[productName];
                productParameters.Price = double.Parse(prod.Element(xmlPrice.Name).Value, CultureInfo.InvariantCulture);
                productParameters.Mean = double.Parse(prod.Element(xmlMean.Name).Value, CultureInfo.InvariantCulture);
                productParameters.StandardDeviation = double.Parse(prod.Element(xmlStandarddeviation.Name).Value, CultureInfo.InvariantCulture);
                (ProductsParameters as IList<ProductParameters>).Add(productParameters);
            }
        }

    }
}
