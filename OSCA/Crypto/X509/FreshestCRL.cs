using System;
using System.Text;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Asn1.X509;

namespace OSCA.Crypto.X509
{
    /// <summary>
    /// Class describing the FreshestCRL extension (aka Delta CRL)
    /// </summary>
    /// <remarks>
    /// This class is derived from the BC CRLDistPoint class as it is basically the same (see RFC 5280):
    /// <code>
    /// id-ce-freshestCRL OBJECT IDENTIFIER ::=  { id-ce 46 }
    /// 
    /// FreshestCRL ::= CRLDistributionPoints
    /// </code>
    /// </remarks>
    public class FreshestCRL : Asn1Encodable
    {
        internal readonly Asn1Sequence seq;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="explicitly">if set to <c>true</c> [explicitly].</param>
        /// <returns></returns>
		public static FreshestCRL GetInstance(
            Asn1TaggedObject	obj,
            bool				explicitly)
        {
            return GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Unknown object in factory</exception>
		public static FreshestCRL GetInstance(
            object obj)
        {
            if (obj is FreshestCRL || obj == null)
            {
                return (FreshestCRL) obj;
            }

			if (obj is Asn1Sequence)
            {
                return new FreshestCRL((Asn1Sequence) obj);
            }

			throw new ArgumentException("unknown object in factory: " + obj.GetType().Name, "obj");
		}

		private FreshestCRL(
            Asn1Sequence seq)
        {
            this.seq = seq;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FreshestCRL"/> class.
        /// </summary>
        /// <param name="points">Distribution points.</param>
		public FreshestCRL(
            DistributionPoint[] points)
        {
			seq = new DerSequence(points);
        }

        /// <summary>
        /// Return the distribution points making up the sequence.
        /// </summary>
        /// <returns>DistributionPoint[]</returns>
        public DistributionPoint[] GetDistributionPoints()
        {
            DistributionPoint[] dp = new DistributionPoint[seq.Count];

			for (int i = 0; i != seq.Count; ++i)
            {
                dp[i] = DistributionPoint.GetInstance(seq[i]);
            }

			return dp;
        }

        /// <summary>
        /// Produce an object suitable for an Asn1OutputStream.
        /// </summary>
        /// <returns>Asn1 Object</returns>
        public override Asn1Object ToAsn1Object()
        {
            return seq;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
		public override string ToString()
		{
			StringBuilder buf = new StringBuilder();
            //string sep = Platform.NewLine; 
            string sep = "\r\n";

			buf.Append("FreshestCRL:");
			buf.Append(sep);
			DistributionPoint[] dp = GetDistributionPoints();
			for (int i = 0; i != dp.Length; i++)
			{
				buf.Append("    ");
				buf.Append(dp[i]);
				buf.Append(sep);
			}
			return buf.ToString();
		}
	}
}
