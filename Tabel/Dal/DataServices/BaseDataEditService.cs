using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tabel.ViewModels;

namespace Tabel.Dal
{
    public abstract class BaseDataEditService <T>: IDisposable where T: IDataEditViewModel
    {
        protected TabelContext _tabelContext;

        protected BaseDataEditService(TabelContext tabelContext)
        {
            _tabelContext = tabelContext;
        }

        public abstract IEnumerable<T> Read();

        public abstract void Create(T dataToCreate);

        public abstract void Update(T dataToUpdate);

        public abstract void Destroy(T dataToDelete);

        public void Dispose()
        {
            _tabelContext.Dispose();
        }

    }
}