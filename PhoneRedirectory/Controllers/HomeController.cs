using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PhoneRedirectory.Models;
using PhoneRedirectory.Models.Data;
using PhoneRedirectory.Models.Entity;
using PhoneRedirectory.Models.Helper;
using PhoneRedirectory.Models.ReqResp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PhoneRedirectory.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;
        private readonly ConnectionStrings _conn;

        public HomeController(DataContext context, IOptions<ConnectionStrings> conn)
        {
            _context = context;
            _conn = conn.Value;
        }

        public IActionResult Index()
        {
            return View(new DataTable());
        }

        [Route("IndexValue")]
        [HttpPost]
        public IActionResult Index(IndexRequest request)
        {
            //List<FullViewObject> vwList = new List<FullViewObject>();
            //vwList = _context.vw_People_PhoneType_No_Country.FromSqlRaw($"SELECT * FROM dbo.vw_People_PhoneType_No_Country (null,{1},null,null)").ToList();

            DataTable dt = new DataTable();

            using (var connection = new SqlConnection(_conn.DefaultConnection))
            {
                #region Get Type
                var vwString = connection.Query<dynamic>($"select [View] from DirectoryTypes where Id={request.TypeId}").ToList();
                #endregion

                //var sql = $"select * from {vwString[0].View} (null,null,null,null)";
                var sql = $"select * from {vwString[0].View} ";
                var filters = request.Filters.OrderBy(x => x.FilterId);
                string filterString = "";

                foreach (var item in filters)
                {
                    if (item.Value != null && item.Value != "")
                    {
                        if (item.FilterTypeId != 4)
                        {
                            filterString = filterString + "\'" + item.Value + "\'" + ",";
                        }
                        else
                            filterString = filterString + "" + item.Value + "" + ",";
                    }
                    else
                        filterString = filterString + "null" + ",";
                }

                sql += "(" + filterString.TrimEnd(',') + ")";

                // Use the Query method to execute the query and return a list of objects    
                //var books = connection.Query<object>(sql).ToList();

                dt = ToDataTable(connection.Query<FullViewObject>(sql).ToList());

                #region trim data table
                foreach (var column in dt.Columns.Cast<DataColumn>().ToArray())
                {
                    if (dt.AsEnumerable().All(dr => dr.IsNull(column)))
                        dt.Columns.Remove(column);
                }

                dt.Columns.Remove(dt.Columns[0]);
                #endregion

                //dt = this.ToDataTable(books);
            }

            //DataTable dt = this.ToDataTable(_context.vw_People_PhoneType_No_Country.FromSqlRaw($"SELECT * FROM dbo.vw_People_PhoneType_No_Country (null,{1},null,null)").ToList());

            //return View(dt);
            return PartialView("Index", dt);
        }

        public IActionResult DirectoryTypesGet()
        {
            DirectoryTypesGetResp resp = new DirectoryTypesGetResp { IsSuccess = true };

            try
            {
                resp.Data = _context.DirectoryTypes.Select(x => new DirectoryTypesGetData { Id = x.Id, Name = x.Name }).ToList();
            }
            catch (Exception ex)
            {
                resp.IsSuccess = false;
            }

            return Ok(resp);
        }

        public IActionResult FiltersGet(int directoryTypeId)
        {
            FiltersGetResponse resp = new FiltersGetResponse { IsSuccess = true };

            try
            {
                resp.Data = (from d in _context.DirectoryTypes
                             join tf in _context.TypeFilter
                             on d.Id equals tf.TypeId
                             join f in _context.Filters
                             on tf.FilterId equals f.Id
                             join ft in _context.FilterType
                             on f.FilterTypeId equals ft.Id
                             where d.Id == directoryTypeId
                             select new FiltersGetData
                             {
                                 FilterId = f.Id,
                                 FilterType = ft.Name,
                                 FilterName = f.Name,
                                 FilterTypeId = ft.Id
                             }).ToList();
            }
            catch (Exception ex)
            {
                resp.IsSuccess = false;
            }

            return Ok(resp);
        }

        public IActionResult AssignCountries()
        {
            AssignCountriesResponse response = new AssignCountriesResponse { IsSuccess = true };

            try
            {
                response.Data = _context.Country.Select(x => new AssignCountriesData { CountryId = x.Id, CountryName = x.Name }).ToList();
            }
            catch (Exception)
            {
                response.IsSuccess = false;
            }

            return Ok(response);
        }

        public IActionResult AssignPhoneTypes()
        {
            AssignPhoneTypesResponse response = new AssignPhoneTypesResponse { IsSuccess = true };

            try
            {
                response.Data = _context.PhoneType.Select(x => new AssignPhoneTypes { PhoneTypesId = x.Id, PhoneTypesName = x.Name }).ToList();
            }
            catch (Exception)
            {
                response.IsSuccess = false;
            }

            return Ok(response);
        }

        public DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }

            return table;
        }
    }
}
