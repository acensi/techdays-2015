using System.Collections.Generic;
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
                productParameters.Price = double.Parse(prod.Element(xmlPrice.Name).Value);
                productParameters.Mean = double.Parse(prod.Element(xmlMean.Name).Value);
                productParameters.StandardDeviation = double.Parse(prod.Element(xmlStandarddeviation.Name).Value);
                (ProductsParameters as IList<ProductParameters>).Add(productParameters);
            }
        }

    }
}
