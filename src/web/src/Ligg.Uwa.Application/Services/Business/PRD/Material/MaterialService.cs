using Ligg.EntityFramework.Entities;
using Ligg.Uwa.Application;
using Ligg.Uwa.Application.Shared;
using Ligg.Infrastructure.Helpers;
using Ligg.Infrastructure.Utilities.DataParserUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ligg.Uwa.Basis.SYS;

namespace Ligg.Uwa.Business.PRD
{
    public class MaterialService
    {

        //*manage
        public async Task<List<MaterialManageDto>> GetManageDtosAsync(CommonReqArgs param)
        {
            if (MaterialDb.MaterialManageDtos.Count != 0)
                return MaterialDb.MaterialManageDtos;

            var cfgHandle = new ConfigHandler();
            var matTypes = cfgHandle.GetConfigItems((int)FuncConfigSubType.MaterialType + "");
            var matType = matTypes.Find(x => x.Id.ToString() == "574696147797413888");
            var typeName = matType == null ? "" : matType.Name;

            var dtos = new List<MaterialManageDto>();
            var ett = new Material("574696147797413888", 10000001, "不锈钢平头尖尾自攻螺丝", "304不锈钢平头尖尾自攻螺丝M5*30", 0);
            var dto = ett.MapTo<MaterialManageDto>();
            dto.BasicUnitText = (EnumHelper.GetNameById<BasiUnit>(ett.BasicUnit)).ToString();
            dto.Type = ett.Type;
            dto.TypeName = typeName;
            dtos.Add(dto);

            ett = new Material("574696147797413888", 10000002, "不锈钢外六角螺栓", "316不锈钢不锈钢外六角螺栓A2-70", 0);
            dto = ett.MapTo<MaterialManageDto>();
            dto.BasicUnitText = (EnumHelper.GetNameById<BasiUnit>(ett.BasicUnit)).ToString();
            dto.TypeName = typeName;
            dto.Type = ett.Type;
            dtos.Add(dto);
            ett = new Material("574696147797413888", 10000003, "金属防锈漆", "金属防锈漆防腐漆绿色DW-385", 1);
            dto = ett.MapTo<MaterialManageDto>();
            dto.BasicUnitText = (EnumHelper.GetNameById<BasiUnit>(ett.BasicUnit)).ToString();
            dto.TypeName = typeName;
            dto.Type = ett.Type;
            dtos.Add(dto);
            ett = new Material("574696147797413888", 10000004, "金属防锈漆", "50cm宽PE大卷缠绕膜工业保鲜膜", 2);
            dto = ett.MapTo<MaterialManageDto>();
            dto.BasicUnitText = (EnumHelper.GetNameById<BasiUnit>(ett.BasicUnit)).ToString();
            dto.TypeName = typeName;
            dto.Type = ett.Type;
            dtos.Add(dto);
            MaterialDb.MaterialManageDtos.AddRange(dtos);
            return MaterialDb.MaterialManageDtos;
        }

        //*save
        public async Task<string> SaveAddEditDtoAsync(Material ett)
        {
            var dto = ett.MapTo<MaterialManageDto>();
            var maxOne = MaterialDb.MaterialManageDtos.OrderByDescending(x=>x.Id).ToList().First();
            dto.Id = maxOne.Id + 1;
            dto.BasicUnitText = (EnumHelper.GetNameById<BasiUnit>(ett.BasicUnit)).ToString();
            var cfgHandle = new ConfigHandler();
            var matTypes = cfgHandle.GetConfigItems((int)FuncConfigSubType.MaterialType + "");
            var matType = matTypes.Find(x => x.Id.ToString() == ett.Type);
            dto.Type = ett.Type;
            dto.TypeName = matType == null ? "" : matType.Name;
            MaterialDb.MaterialManageDtos.Add(dto);
            return Consts.OK;
        }


    }

    public class Material
    {
        public Material(string type, int id, string name, string description, int basicUnit)
        {
            this.Type = type;
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.BasicUnit = basicUnit;
        }
        public Material()
        {
        }
        public string Type { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BasicUnit { get; set; }
    }
    public class MaterialManageDto
    {
        public int Id;
        public string TypeName { get; set; }
        public string Name;
        public string Description;
        public string BasicUnitText;
        public string Type { get; set; }
    }
    public enum BasiUnit
    {
        Pcs = 0, Kg = 1, M = 2, M2 = 3, M3 = 4
    }

    public class MaterialDb
    {
        public static List<MaterialManageDto> MaterialManageDtos= new List < MaterialManageDto > ();
    }


}
