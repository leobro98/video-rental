﻿using Leobro.VideoStore;
using Leobro.VideoStore.Data;
using Leobro.VideoStore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leobro.VideoStoreTest
{
    class RepositorySpy : SimpleRepository
    {
        public List<VideoTitle> Titles { get { return titles; } }
        public List<Casette> Casettes { get { return casettes; } }
        public List<Customer> Customers { get { return customers; } }
        public List<Rental> Rentals { get { return rentals; } }
    }
}