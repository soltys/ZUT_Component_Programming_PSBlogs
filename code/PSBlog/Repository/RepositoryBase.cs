﻿using Ninject;
using Ninject.Extensions.Logging;
using Ninject.Extensions;
using PSBlog.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace PSBlog.Repository
{
    internal abstract class RepositoryBase<T> where T : class
    {       
        
        protected PSBlogContext _db;
        private bool _disposed;
        private readonly ILogger _log;
        public RepositoryBase(PSBlogContext db)
        {
            _db = db;
            var logfac = DependencyResolver.Current.GetService<ILoggerFactory>();
            _log = logfac.GetCurrentClassLogger();
        }

        public IList<T> FetchAll()
        {
            return _db.Set<T>().ToList();
        }

        public void Add(T entity)
        {
            _db.Set<T>().Add(entity);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);

            // Call SupressFinalize in case a subclass implements a finalizer.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_db != null)
                    {
                        _db.Dispose();
                        _log.Info("Repository Disposed " + ToString());
                    }
                }

                _db = null;
                // Indicate that the instance has been disposed.
                _disposed = true;
            }
        }
    }
}