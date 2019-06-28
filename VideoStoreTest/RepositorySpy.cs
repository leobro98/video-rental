using Leobro.VideoStore.Data;
using Leobro.VideoStore.Model;
using System.Collections.Generic;

namespace Leobro.VideoStoreTest
{
    /// <summary>
    /// Exposes internal members of the <see cref="SimpleRepository"/>.
    /// </summary>
    class RepositorySpy : SimpleRepository
    {
        public List<VideoTitle> Titles { get { return titles; } }
        public List<Casette> Casettes { get { return casettes; } }
        public List<Customer> Customers { get { return customers; } }
        public List<Rental> Rentals { get { return rentals; } }
    }
}
