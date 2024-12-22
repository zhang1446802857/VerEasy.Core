using AutoMapper;
using VerEasy.Core.Models.ViewModels;
using static VerEasy.Core.Models.Dtos.ParamDto;
using static VerEasy.Core.Models.Dtos.ResultDto;

namespace VerEasy.Extensions.ServiceExtensions.Module
{
    /// <summary>
    /// automapper映射转换配置
    /// </summary>
    public class MappingProfileModule : Profile
    {
        public MappingProfileModule()
        {
            CreateMap<DepartmentParam, Department>()
                .ForMember(x => x.CreateTime, a => a.Ignore());
            CreateMap<Department, DepartmentResult>();
            CreateMap<User, UserResult>();
            CreateMap<UserParam, User>()
                .ForMember(x => x.LoginPwd, a => a.MapFrom(c => c.Pwd))
                .ForMember(x => x.UserName, a => a.MapFrom(c => string.IsNullOrEmpty(c.UserName) ? Guid.NewGuid().ToString() : c.UserName));
            CreateMap<RoleParam, Role>();
            CreateMap<Role, RoleResult>();
            CreateMap<PermissionParam, Permission>()
                .ForMember(x => x.Type, a => a.MapFrom(c => c.Type));
            CreateMap<Permission, PermissionResult>();
            CreateMap<User, UserCenterResult>();
            CreateMap<EditUserCenterParam, User>()
                .ForAllMembers(x => x.Condition((a, b, c) => c != null));
            CreateMap<QzJobPlan, QzJobPlanResult>();
            CreateMap<QzJobParam, QzJobPlan>();
        }
    }
}