using Emdat.InVision.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emdat.InVision
{
    public class AssociateItem
    {
        /// <summary>
        /// Searches the associates.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="assocFirstName">First name of the assoc.</param>
        /// <param name="assocLastName">Last name of the assoc.</param>
        /// <param name="assocClientCode">The assoc client code.</param>
        /// <returns></returns>
        public static IEnumerable<AssociateItem> Search(int clientId, string assocFirstName, string assocLastName, string assocClientCode)
        {
            using (var data = new Emdat.InVision.Sql.ReportingDataContext())
            {
                var assocs =
                    (from a in data.SearchAssociates2(clientId, assocFirstName, assocLastName, assocClientCode, false)
                     select new AssociateItem
                     {
                         Id = a.AssociateID.ToString(),
                         Name = string.Format("{0}, {1}{2}",
                             a.AssociateNameLast,
                             a.AssociateNameFirst,
                             !string.IsNullOrEmpty(a.AssociateNameMiddle) ? string.Format(" {0}.", a.AssociateNameMiddle) : string.Empty),
                         Address = string.Join(" ", new string[] { a.AssociateAddress1, a.AssociateAddress2 }),
                         BusinessName = a.AssociateBusinessName,
                         City = a.AssociateCity,
                         Specialty = a.AssociateSpecialty,
                         State = a.AssociateState
                     });
                return assocs.ToList();
            }
        }

        /// <summary>
        /// Loads the specified associate id.
        /// </summary>
        /// <param name="associateId">The associate id.</param>
        /// <returns></returns>
        public static AssociateItem Load(string associateId)
        {
            int assocId = int.Parse(associateId);

            using (var data = new Emdat.InVision.Sql.ReportingDataContext())
            {
                var assoc =
                    from a in data.GetAssociate2(assocId)
                    select new AssociateItem
                    {
                        Id = a.AssociateID.ToString(),
                        Name = string.Format("{0}, {1}{2}",
                            a.AssociateNameLast,
                            a.AssociateNameFirst,
                            !string.IsNullOrEmpty(a.AssociateNameMiddle) ? string.Format(" {0}.", a.AssociateNameMiddle) : string.Empty),
                        Address = string.Join(" ", new string[] { a.AssociateAddress1, a.AssociateAddress2 }),
                        BusinessName = a.AssociateBusinessName,
                        City = a.AssociateCity,
                        Specialty = a.AssociateSpecialty,
                        State = a.AssociateState
                    };
                return assoc.FirstOrDefault();
            }
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string BusinessName { get; set; }
        public string Specialty { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
