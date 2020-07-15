using System;
using System.Collections.Generic;
using System.Text;
using System.Xaml.Permissions;
using WebApiUtilities.Abstract;

namespace WebApiUtilities.CrudRequests
{
    public class CRUD<T, TId>
        where T : Entity<TId>
    {
        //public class Create : ICreateCommand<T, TId>
        //{ 
        //}

        //public class GetEntities : IGetEntities<T, TId> { }

        //public class GetEntityById : IGetEntityById<T, TId>
        //{
        //    public TId Id { get; set; }

        //    public GetEntityById(TId Id)
        //    {
        //        this.Id = Id;
        //    }
        //}
        //public class DeleteEntity : IDeleteEntity<T, TId>
        //{
        //    public TId Id { get; set; }

        //    public DeleteEntity(TId Id)
        //    {
        //        this.Id = Id;
        //    }
        //}

        //public class UpdateEntity : IUpdateCommand<T, TId>
        //{
        //    public TId Id { get; set; }
        //}

    }
}
