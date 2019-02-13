using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BreakAway.Domain.Core.Sites;
using BreakAway.Models.Sites;
using ITCloud.Web.Routing;
using BreakAway.Domain.Core.Institutes;
using BreakAway.Domain.Core.Educations;

namespace BreakAway.Controllers
{
    public class SiteController : Controller
    {
        private readonly ISiteRepository _siteRepository;
        private readonly IInstituteRepository _instituteRepository;
        private readonly IEducationRepository _educationRepository;

        public SiteController(ISiteRepository siteRepository, 
            IInstituteRepository instituteRepository,
            IEducationRepository educationRepository)
        {
            _siteRepository = siteRepository;// ?? throw new ArgumentNullException(nameof(siteRepository));
            _instituteRepository = instituteRepository;
            _educationRepository = educationRepository;
        }

        [UrlRoute(Path = "site/list")]
        public ActionResult Index()
        {
            var viewModel = new IndexViewModel();
            viewModel.Sites = _siteRepository.Items
                .Select(p => new SiteItem
            {
                Id = p.Id,
                Domain = p.Domain,
                IsActive = p.IsActive,
                Type = p.Type.Name
            }).ToArray();

            return View(viewModel);
        }

        [UrlRoute(Path = "site/institutes")]
        public ActionResult Institutes(int id)
        {
            var viewModel = new InstituteViewModel();
            viewModel.Institutes = _instituteRepository.Items
                .Where(i=>i.SiteId == id)
                .Select(p => new InstituteItem
            {
                Id = p.Id,
                Name = p.Name,
                Url = p.WWW,
                Email = p.EMail,
                SiteName = p.Site.Domain
            }).ToArray();
            
            return View(viewModel);
        }

        [UrlRoute(Path = "site/educations")]
        public ActionResult Educations(int id)
        {
            var viewModel = new EducationViewModel();
            viewModel.Educations = _educationRepository.Items
                .Where(i => i.InstituteId == id)
                .Select(p => new EducationItem
            {
                Id = p.Id,
                Name = p.Name,
                Url = p.Link,
                InstituteName = p.Institute.Name
            }).ToArray();

            return View(viewModel);
        }
    }
}