using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;



namespace OSCA.Crypto.X509
{
    /// <summary>
    /// Defines the SubjectInformationAccess extension
    /// This is an addition to the BC crypto library
    /// </summary>
    public class SubjectInformationAccess : Asn1Encodable
    {

	/**
	 * The SubjectInformationAccess object.
        id-pe-subjectInfoAccess OBJECT IDENTIFIER ::= { id-pe 11 }

        SubjectInfoAccessSyntax  ::=
           SEQUENCE SIZE (1..MAX) OF AccessDescription

         AccessDescription  ::=  SEQUENCE {
           accessMethod          OBJECT IDENTIFIER,
           accessLocation        GeneralName  }

        id-ad OBJECT IDENTIFIER ::= { id-pkix 48 }

        id-ad-caRepository OBJECT IDENTIFIER ::= { id-ad 5 }

        id-ad-timeStamping OBJECT IDENTIFIER ::= { id-ad 3 }
	 */

		private readonly AccessDescription[] descriptions;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>An instance of the SubjectInformationAccess class</returns>
        /// <exception cref="System.ArgumentException">unknown object in factory</exception>
		public static SubjectInformationAccess GetInstance(
			object obj)
		{
			if (obj is SubjectInformationAccess)
				return (SubjectInformationAccess) obj;

			if (obj is Asn1Sequence)
				return new SubjectInformationAccess((Asn1Sequence) obj);

			if (obj is X509Extension)
				return GetInstance(X509Extension.ConvertValueToObject((X509Extension) obj));

			throw new ArgumentException("unknown object in factory: " + obj.GetType().Name, "obj");
		}

		private SubjectInformationAccess(
			Asn1Sequence seq)
		{
			if (seq.Count < 1)
				throw new ArgumentException("sequence may not be empty");

			this.descriptions = new AccessDescription[seq.Count];

			for (int i = 0; i < seq.Count; ++i)
			{
				descriptions[i] = AccessDescription.GetInstance(seq[i]);
			}
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="SubjectInformationAccess"/> class.
        /// </summary>
        /// <param name="oid">The oid.</param>
        /// <param name="location">The location.</param>
		[Obsolete("Use version taking an AccessDescription instead")]
		public SubjectInformationAccess(
			DerObjectIdentifier	oid,
			GeneralName			location)
		{
			this.descriptions = new AccessDescription[]{ new AccessDescription(oid, location) };
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="SubjectInformationAccess"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
		public SubjectInformationAccess(
			AccessDescription description)
		{
			this.descriptions = new AccessDescription[]{ description };
		}

        /// <summary>
        /// Gets the access descriptions.
        /// </summary>
        /// <returns></returns>
		public AccessDescription[] GetAccessDescriptions()
		{
			return (AccessDescription[]) descriptions.Clone();
		}

        /// <summary>
        /// To the asn1 object.
        /// </summary>
        /// <returns></returns>
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(descriptions);
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
            string sep = Environment.NewLine;

			buf.Append("SubjectInformationAccess:");
			buf.Append(sep);

			foreach (AccessDescription description in descriptions)
			{
				buf.Append("    ");
				buf.Append(description);
				buf.Append(sep);
			}

			return buf.ToString();
		}
	}
}
