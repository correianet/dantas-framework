using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web;
using System.Web.SessionState;

namespace Dantas.Support
{
    /// <summary>
    /// Use session for cache elements.
    /// </summary>
    public class SessionCache : ICollection
    {
        #region Singleton

        [ThreadStatic]
        private static SessionCache instance;
        private static SessionCache GenerateInstance()
        {
            string sessionKey = "InstanceSystemCacheForApplication";

            SessionCache result;
            if (HttpContext.Current == null)
            {
                result = instance ?? (instance = new SessionCache(false));
            }
            else
            {
                if (HttpContext.Current.Session[sessionKey] == null)
                {
                    HttpContext.Current.Session.Add(sessionKey, new SessionCache(true));
                }
                result = (SessionCache)HttpContext.Current.Session[sessionKey];
            }

            return result;
        }

        /// <summary>
        /// Current instance.
        /// </summary>
        public static SessionCache Current
        {
            get { return GenerateInstance(); }
        }

        #endregion

        private bool webEnvironment = false;
        private ICollection hash;

        private SessionCache(bool webEnvironment)
        {
            this.webEnvironment = webEnvironment;
            if (this.webEnvironment)
                this.hash = HttpContext.Current.Session;
            else
                this.hash = new Hashtable();
        }

        /// <summary>
        /// Get element by name key.
        /// </summary>
        /// <param name="name">Name key.</param>
        /// <returns>Element object.</returns>
        public object this[string name]
        { 
            get
            {
                object result;

                if (this.webEnvironment)
                    result = ((HttpSessionState)this.hash)[name];
                else
                    result = ((Hashtable)this.hash)[name];

                return result;
            }
            set
            {
                if (this.webEnvironment)
                    ((HttpSessionState)this.hash).Add(name, value);
                else
                    ((Hashtable)this.hash).Add(name, value);
            }
        }

        /// <summary>
        /// Add element.
        /// </summary>
        /// <param name="name">Name key.</param>
        /// <param name="value">Element value.</param>
        public void Add(string name, object value)
        {
            if (this.webEnvironment)
                ((HttpSessionState)this.hash).Add(name, value);
            else
                ((Hashtable)this.hash).Add(name, value);
        }

        /// <summary>
        /// Remove element.
        /// </summary>
        /// <param name="name">Name key.</param>
        public void Remove(string name)
        {
            if (this.webEnvironment)
                ((HttpSessionState)this.hash).Remove(name);
            else
                ((Hashtable)this.hash).Remove(name);
        }

        #region ICollection Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Array array, int index)
        {
            this.hash.CopyTo(array, index);
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return this.hash.Count; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSynchronized
        {
            get { return this.hash.IsSynchronized; }
        }

        /// <summary>
        /// 
        /// </summary>
        public object SyncRoot
        {
            get { return this.hash.SyncRoot; }
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return this.hash.GetEnumerator();
        }

        #endregion
    }
}
