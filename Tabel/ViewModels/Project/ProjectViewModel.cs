using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tabel.ViewModels.Project
{
    public class ProjectViewModel : IDataEditViewModel
    {
        public int ProjectId { get; set; }
        public string Code { get; set; }

    }
}