using Org.BouncyCastle.Asn1;

namespace OSCA.Crypto.X509
{
    class X509Name : Org.BouncyCastle.Asn1.X509.X509Name
    {      

        /// <param name="other">The X509Name object to test equivalency against.</param>
		/// <param name="inOrder">If true, the order of elements must be the same,
		/// as well as the values associated with each element.</param>
		public string EquivalentwithErrors(
			X509Name	other,
			bool		inOrder)
		{
			if (!inOrder)
				return this.Equivalent(other);

			if (other == null)
				return false;

			if (other == this)
				return true;

			int orderingSize = ordering.Count;

			if (orderingSize != other.ordering.Count)
				return false;

			for (int i = 0; i < orderingSize; i++)
			{
				DerObjectIdentifier oid = (DerObjectIdentifier) ordering[i];
				DerObjectIdentifier oOid = (DerObjectIdentifier) other.ordering[i];

				if (!oid.Equals(oOid))
					return false;

				string val = (string) values[i];
				string oVal = (string) other.values[i];

				if (!equivalentStrings(val, oVal))
					return false;
			}

			return true;
		}

    
    }
}
