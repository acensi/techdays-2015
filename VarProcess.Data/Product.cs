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

using System;

namespace VarProcess.Data
{
    public sealed class Product : IEquatable<Product>
    {
        public string Name { get; set; }

        public bool Equals(Product other)
        {
            return other.Name == Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var prd = obj as Product;
            if (prd != null)
            {
                return Equals(prd);
            }
            return false;
        }
    }
}
