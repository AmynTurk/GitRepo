using PhoneRedirectory.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneRedirectory.Models.ReqResp
{
    public class Home
    {
    }

    public class DirectoryTypesGetResp : ResponseBase
    {
        public List<DirectoryTypesGetData> Data { get; set; }
    }

    public class DirectoryTypesGetData
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class FiltersGetResponse:ResponseBase
    {
        public List<FiltersGetData> Data { get; set; }
    }

    public class FiltersGetData
    {
        public int FilterId { get; set; }
        public string FilterName { get; set; }
        public string FilterType { get; set; }
        public int FilterTypeId { get; internal set; }
    }

    public class IndexRequest
    {
        public int TypeId { get; set; }
        public List<FiltersDTO> Filters { get; set; }
    }

    public class FiltersDTO
    {
        public int FilterId { get; set; }
        public string Value { get; set; }
        public int FilterTypeId { get; set; }
    }

    public class AssignCountriesResponse : ResponseBase
    {
        public List<AssignCountriesData> Data { get; set; }
    }

    public class AssignCountriesData
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }

    public class AssignPhoneTypesResponse : ResponseBase
    {
        public List<AssignPhoneTypes> Data { get; set; }
    }

    public class AssignPhoneTypes
    {
        public int PhoneTypesId { get; set; }
        public string PhoneTypesName { get; set; }
    }
}
