using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dantas.Data.Util
{
    /// <summary>
    /// Implements this for custom data objects.
    /// </summary>
    /// <typeparam name="T">Type of entity</typeparam>
    public interface IDataObject<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T[] Read(object id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        T[] ReadAll();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        T[] ReadAll(int startIndex, int length);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sorter"></param>
        /// <returns></returns>
        T[] ReadAll(string sorter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <param name="sorter"></param>
        /// <returns></returns>
        T[] ReadAll(int startIndex, int length, string sorter);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transferObject"></param>
        void Update(T transferObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transferObject"></param>
        void Create(T transferObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transferObject"></param>
        void Delete(T transferObject);
    }
}
