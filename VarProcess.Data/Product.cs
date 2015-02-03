using System;

namespace VarProcess.Data
{
    public sealed class Product : IEquatable<Product>
    {
        public string Name { get; set; }

        public bool Equals(Product other)
        {
            if(other.Name == Name)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Product prd = obj as Product;
            if (prd != null)
            {
                return Equals(prd);
            }
            else
            {
                return false;
            }
        }
    }
}
