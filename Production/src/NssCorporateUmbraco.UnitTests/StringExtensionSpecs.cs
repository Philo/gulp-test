using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NssCorporateUmbraco.Code.Base.Extensions;
using NUnit.Framework;
using Storm.Testing.Specification;

namespace NssCorporateUmbraco.UnitTests
{

    public class StringExtensionSpecs
    {
        [TestFixture]
        public class WhenGeneratingCssClass : ContextSpecification
        {
            private string result;
            
            protected override void Because()
            {
                result = "my test string".ToCssClassName();
            }

            [Test]
            public void ShouldReturnCorrectCssClass()
            {
                result.ShouldEqual("mts");
            }
        }

    }
}
