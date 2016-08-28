using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tabel.Models;
using Tabel.ViewModels.Project;

namespace Tabel.Dal.DataServices
{
    public class ProjectService : BaseDataEditService<ProjectViewModel>
    {
        public ProjectService(TabelContext tabelContext) : base(tabelContext)
        {
        }

        public override IEnumerable<ProjectViewModel> Read()
        {
            var result = _tabelContext.Projects.Select(prj => new ProjectViewModel
            {
                Code = prj.Name,
                ProjectId = prj.Id
            });
            return result;
        }

        public override void Create(ProjectViewModel dataToCreate)
        {
            var project = new Project
            {
                Name = dataToCreate.Code,
            };
            _tabelContext.Projects.Add(project);
            _tabelContext.SaveChanges();
        }

        public override void Update(ProjectViewModel dataToUpdate)
        {
            var edited = _tabelContext.Projects.FirstOrDefault(prj => prj.Id == dataToUpdate.ProjectId);
            if (edited == null)
                return;
            edited.Name = dataToUpdate.Code;
            _tabelContext.SaveChanges();
        }

        public override void Destroy(ProjectViewModel dataToDelete)
        {
            var projectToRemove = _tabelContext.Projects.FirstOrDefault(prj => prj.Id == dataToDelete.ProjectId);
            if (projectToRemove == null)
                return;

            var linkedTimesheets =
                _tabelContext.Timesheets.Include("Project").Where(ts => ts.Project.Id == projectToRemove.Id);

            _tabelContext.Timesheets.RemoveRange(linkedTimesheets);
            _tabelContext.Projects.Remove(projectToRemove);


            _tabelContext.SaveChanges();
        }
    }
}