using Leobro.VideoStore.Model;
using System;

namespace Leobro.VideoStoreTest
{
    class TestHelper
    {
        private RepositorySpy repo;

        public TestHelper(RepositorySpy repository)
        {
            repo = repository;
        }

        public VideoTitle AddNewTestTitleWithOneCasette(Casette.CasetteStatus casetteStatus)
        {
            var title = GetBrandNewTitle();
            repo.Titles.Add(title);

            var casette = new Casette(repo.Casettes.Count + 1, title);
            casette.Status = casetteStatus;
            repo.Casettes.Add(casette);

            return title;
        }

        public VideoTitle GetBrandNewTitle()
        {
            string name = Guid.NewGuid().ToString();
            int year = DateTime.Now.Year;
            return new VideoTitle(name, year, repo.Titles.Count + 1);
        }
    }
}
