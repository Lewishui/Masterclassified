using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterClassified
{
    public class MockEntity : IEquatable<MockEntity>
    {
        public int Id { get; set; }
        public int TaxonId { get; set; } // genreId
        public string FullName { get; set; }
        public string ShortName { get; set; }

        //https://msdn.microsoft.com/zh-cn/library/bb348436(v=vs.110).aspx
        public bool Equals(MockEntity other)
        {

            //Check whether the compared object is null. 
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data. 
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal. 
            return Id.Equals(other.Id);
        }

        // If Equals() returns true for a pair of objects  
        // then GetHashCode() must return the same value for these objects. 

        public override int GetHashCode()
        {

            //Get hash code for the Name field if it is not null. 
            int hashProductName = ShortName == null ? 0 : ShortName.GetHashCode();

            //Get hash code for the Code field. 
            int hashProductCode = Id.GetHashCode();

            //Calculate the hash code for the product. 
            return hashProductName ^ hashProductCode;
        }
    }
}
