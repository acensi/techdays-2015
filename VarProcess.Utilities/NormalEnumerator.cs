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

using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.Distributions;

namespace VarProcess.Utilities
{
    public class NormalEnumerator : IEnumerator<double>
    {
        private Normal _distrib = new Normal();
        private double _current = 0.0;

        public void Dispose()
        {

        }

        public bool MoveNext()
        {
            _current = _distrib.Sample();
            return true;
        }

        public void Reset()
        {
            _distrib = new Normal();
        }

        public double Current
        {
            get
            {
                return _current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }
    }
}