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

        public VideoTitle AddNewTestTitleWithOneCasette()
        {
            var title = GetBrandNewTitle();
            repo.Titles.Add(title);

            var casette = new Casette(repo.Casettes.Count + 1, title);
            repo.Casettes.Add(casette);

            return title;
        }

        public VideoTitle GetBrandNewTitle()
        {
            string name = Guid.NewGuid().ToString();
            int year = DateTime.Now.Year;
            return new VideoTitle(name, year, repo.Titles.Count + 1);
        }

        public RentalOptions CreateEmptyOptions()
        {
            return new RentalOptions(VideoTitle.TitleType.New, 0, 0, false, 0);
        }
    }
}
