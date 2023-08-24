using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Login_MVC_
{

    public class Rootobject
    {
        public string odatacontext { get; set; }
        public Value[] value { get; set; }
    }

    public class Value
    {
        public string odataetag { get; set; }
        public string dcs_employeeid { get; set; }
        public string _owningbusinessunit_value { get; set; }
        public string dcs_usermanagementid { get; set; }
        public int statecode { get; set; }
        public int statuscode { get; set; }
        public string _createdby_value { get; set; }
        public string dcs_username { get; set; }
        public string _ownerid_value { get; set; }
        public DateTime modifiedon { get; set; }
        public string _modifiedby_value { get; set; }
        public string _owninguser_value { get; set; }
        public DateTime createdon { get; set; }
        public int versionnumber { get; set; }
        public string dcs_password { get; set; }
        public string dcs_workerid { get; set; }
        public object overriddencreatedon { get; set; }
        public object importsequencenumber { get; set; }
        public object _modifiedonbehalfby_value { get; set; }
        public object utcconversiontimezonecode { get; set; }
        public object _createdonbehalfby_value { get; set; }
        public object _owningteam_value { get; set; }
        public object timezoneruleversionnumber { get; set; }
    }

}