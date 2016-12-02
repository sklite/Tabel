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
                Code = prj.Code,
                WorkObject = prj.WorkObject,
                Name = prj.Name,
                ProjectId = prj.Id
            });
            return result;
        }

        public override void Create(ProjectViewModel dataToCreate)
        {
            if (_tabelContext.Projects.Any(prj => prj.Code == dataToCreate.Code))
                return;
            var project = new Project
            {
                Code = dataToCreate.Code,
                Name = dataToCreate.Name,
                WorkObject = dataToCreate.WorkObject
            };
            _tabelContext.Projects.Add(project);
            _tabelContext.SaveChanges();
        }

        public override void Update(ProjectViewModel dataToUpdate)
        {
            var edited = _tabelContext.Projects.FirstOrDefault(prj => prj.Id == dataToUpdate.ProjectId);
            if (edited == null)
                return;
            edited.Code = dataToUpdate.Code;
            edited.Name = dataToUpdate.Name;
            edited.WorkObject = dataToUpdate.WorkObject;
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